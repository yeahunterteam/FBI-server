/*
 * This file is part of FBI.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
 * 
 * FBI is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * FBI is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with FBI.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using FBI.Framework.Irc;
using FBI.Framework.Clean;
using FBI.Framework.Config;
using FBI.Framework.Network;
using FBI.Framework.Database;
using FBI.Framework.Database.Cache;
using FBI.Framework.Functions;
using FBI.Framework.Extensions;
using FBI.Framework.Localization;

namespace FBI.Framework
{
	public sealed class FBIBase
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly object WriteLock = new object();
		private static readonly Guid _guid = Guid.NewGuid();
		public static CleanManager sCleanManager { get; private set; }
		public static DatabaseManager DManager { get; private set; }
		public static ServerListener sListener { get; private set; }
		public static CacheDB sCacheDB { get; private set; }
		public static Timer timer { get; private set; }
		public const string Title = "FBI IRC Bot and Framework";
		public static bool ExitStatus { get; private set; }
		public static bool UrlTitleEnabled = false;
		public static bool NewNick = false;
		public static bool STime = true;
		public const string On = "on";
		public const string Off = "off";
		public const char NewLine = '\n';
		public const char Space = ' ';
		public const char Comma = ',';
		public const char Point = '.';
		public const char Colon = ':';
		public static string PidFile;

		/// <summary>
		/// Gets the GUID.
		/// </summary>
		public static Guid GetGuid() { return _guid; }

		public FBIBase()
		{
			try
			{
				ExitStatus = false;

				if(ServerConfig.Enabled)
				{
					sListener = new ServerListener(ServerConfig.Port);
					new Thread(() => sListener.Listen()).Start();
				}

				if(sUtilities.GetPlatformType() == PlatformType.Linux)
					System.Net.ServicePointManager.ServerCertificateValidationCallback += (s,ce,ca,p) => true;

				Log.Debug("FBIBase", sLConsole.FBIBase("Text"));
				timer = new Timer();

				Log.Debug("FBIBase", sLConsole.FBIBase("Text2"));
				DManager = new DatabaseManager();

				Log.Debug("FBIBase", sLConsole.FBIBase("Text8"));
				sCacheDB = new CacheDB();
				sCacheDB.Load();

				Log.Notice("FBIBase", sLConsole.FBIBase("Text3"));
				sLManager.Locale = LocalizationConfig.Locale;

				var db0 = DManager.Query("SELECT ServerName FROM servers");
				if(!db0.IsNull())
				{
					foreach(DataRow row in db0.Rows)
					{
						string name = row["ServerName"].ToString();
						ServerList.List.Add(name, new IrcServer(name));
					}
				}
				else
					Log.Warning("FBIBase", "Nem áll rendelkezésre irc szerver amit be lehetne tölteni!");

				foreach(var sn in ServerList.List)
				{
					DManager.Update("channels", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId()));
					DManager.Update("fbi", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId()));

					NewServerSqlData(sn.Value.ServerId(), sn.Key);
					IsAllFBIFunction(sn.Value.ServerId(), sn.Key);
					IsAllChannelFunction(sn.Value.ServerId());

					var db = DManager.Query("SELECT FunctionName, FunctionStatus FROM fbi WHERE ServerName = '{0}'", sn.Key);
					if(!db.IsNull())
					{
						var list = new Dictionary<string, string>();

						foreach(DataRow row in db.Rows)
						{
							string name = row["FunctionName"].ToString();
							string status = row["FunctionStatus"].ToString();
							list.Add(name.ToLower(), status.ToLower());
						}

						IFunctionsClass.ServerList.Add(sn.Key, new IFunctionsClassBase(list));
					}
					else
						Log.Error("FBIBase", sLConsole.ChannelInfo("Text11"));
				}

				Log.Debug("FBIBase", sLConsole.FBIBase("Text9"));
				sCleanManager = new CleanManager();
				sCleanManager.Initialize();
			}
			catch(Exception e)
			{
				Log.Error("FBIBase", sLConsole.Exception("Error"), e.Message);
			}
		}

		/// <summary>
		///     Ha lefut, akkor leáll a class.
		/// </summary>
		~FBIBase()
		{
			Log.Debug("FBIBase", "~FBIBase()");
		}

		public static void Quit(bool Reconnect = true)
		{
			lock(WriteLock)
			{
				if(ExitStatus)
					return;

				ExitStatus = true;
				var memory = Process.GetCurrentProcess().WorkingSet64;
				sUtilities.RemovePidFile();
				timer.SaveUptime(memory);
				sCacheDB.Clean();
			}
		}

		private void NewServerSqlData(int ServerId, string ServerName)
		{
			var db = DManager.QueryFirstRow("SELECT * FROM fbi WHERE ServerId = '{0}'", ServerId);
			if(db.IsNull())
			{
				foreach(var function in Enum.GetNames(typeof(IFunctions)))
					DManager.Insert("`fbi`(ServerId, ServerName, FunctionName, FunctionStatus)", ServerId, ServerName, function.ToLower(), On);
			}
		}

		private void IsAllFBIFunction(int ServerId, string ServerName)
		{
			foreach(var function in Enum.GetNames(typeof(IFunctions)))
			{
				var db = DManager.QueryFirstRow("SELECT * FROM fbi WHERE ServerId = '{0}' And FunctionName = '{1}'", ServerId, function.ToLower());
				if(db.IsNull())
					DManager.Insert("`fbi`(ServerId, ServerName, FunctionName, FunctionStatus)", ServerId, ServerName, function.ToLower(), On);
			}
		}

		private void IsAllChannelFunction(int ServerId)
		{
			var db = DManager.Query("SELECT Functions, Channel FROM channels WHERE ServerId = '{0}'", ServerId);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string functions = string.Empty;
					var dic = new Dictionary<string, string>();
					string Channel = row["Channel"].ToString();
					string[] f = row["Functions"].ToString().Split(FBIBase.Comma);
					
					foreach(var ff in f)
					{
						if(ff == string.Empty)
							continue;
						
						if(!ff.Contains(FBIBase.Colon.ToString()))
							continue;
						
						string name = ff.Substring(0, ff.IndexOf(FBIBase.Colon));
						string status = ff.Substring(ff.IndexOf(FBIBase.Colon)+1);
						dic.Add(name, status);
					}
					
					foreach(var function in Enum.GetNames(typeof(IChannelFunctions)))
					{
						if(dic.ContainsKey(function.ToString().ToLower()))
							functions += FBIBase.Comma + function.ToString().ToLower() + FBIBase.Colon + dic[function.ToString().ToLower()];
						else
						{
							if(function == IChannelFunctions.Log.ToString() || function == IChannelFunctions.Rejoin.ToString() ||
							   function == IChannelFunctions.Commands.ToString())
								functions += FBIBase.Comma + function.ToString().ToLower() + FBIBase.Colon + FBIBase.On;
							else
								functions += FBIBase.Comma + function.ToString().ToLower() + FBIBase.Colon + FBIBase.Off;
						}
					}
					
					FBIBase.DManager.Update("channels", string.Format("Functions = '{0}'", functions), string.Format("Channel = '{0}' And ServerId = '{1}'", Channel, ServerId));
				}
			}
		}
	}
}