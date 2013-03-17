/*
 * This file is part of FBI.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
 * Copyright (C) 2012 Jackneill
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
using System.IO;
using System.Text;
using System.Collections.Generic;
using FBI.Framework.Extensions;
using FBI.Framework.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace FBI.Framework.Config
{
	public sealed class YamlConfig : DefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Dictionary<YamlNode, YamlNode> NullYMap = null;

		public YamlConfig()
		{
		}

		public YamlConfig(string configdir, string configfile, bool colorbindmode)
		{
			var yaml = new YamlStream();
			yaml.Load(File.OpenText(sUtilities.DirectoryToSpecial(configdir, configfile)));

			var FBImap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("FBI")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["FBI".ToYamlNode()]).Children : NullYMap;
			LogMap((!FBImap.IsNull() && FBImap.ContainsKey("Log")) ? ((YamlMappingNode)FBImap["Log".ToYamlNode()]).Children : NullYMap);
			Log.Initialize(LogConfig.FileName, colorbindmode);
			Log.Debug("YamlConfig", ">> {0}", configfile);

			Log.Notice("YamlConfig", sLConsole.Config("Text3"));
			ServerMap((!FBImap.IsNull() && FBImap.ContainsKey("Server")) ? ((YamlMappingNode)FBImap["Server".ToYamlNode()]).Children : NullYMap);

			if((!FBImap.IsNull() && FBImap.ContainsKey("Irc")))
			{
				var list = new Dictionary<YamlNode, YamlNode>();

				foreach(var irc in FBImap)
				{
					if(irc.Key.ToString().Contains("Irc"))
						list.Add(irc.Key, irc.Value);
				}

				IrcMap(list);
			}
			else
				IrcMap(NullYMap);

			MySqlMap((!FBImap.IsNull() && FBImap.ContainsKey("MySql")) ? ((YamlMappingNode)FBImap["MySql".ToYamlNode()]).Children : NullYMap);
			SQLiteMap((!FBImap.IsNull() && FBImap.ContainsKey("SQLite")) ? ((YamlMappingNode)FBImap["SQLite".ToYamlNode()]).Children : NullYMap);
			CrashMap((!FBImap.IsNull() && FBImap.ContainsKey("Crash")) ? ((YamlMappingNode)FBImap["Crash".ToYamlNode()]).Children : NullYMap);
			LocalizationMap((!FBImap.IsNull() && FBImap.ContainsKey("Localization")) ? ((YamlMappingNode)FBImap["Localization".ToYamlNode()]).Children : NullYMap);
			ShutdownMap((!FBImap.IsNull() && FBImap.ContainsKey("Shutdown")) ? ((YamlMappingNode)FBImap["Shutdown".ToYamlNode()]).Children : NullYMap);
			CleanMap((!FBImap.IsNull() && FBImap.ContainsKey("Clean")) ? ((YamlMappingNode)FBImap["Clean".ToYamlNode()]).Children : NullYMap);

			Log.Success("YamlConfig", sLConsole.Config("Text4"));
			Console.WriteLine();
		}

		~YamlConfig()
		{
		}

		public bool CreateConfig(string ConfigDirectory, string ConfigFile, bool ColorBindMode)
		{
			try
			{
				string filename = sUtilities.DirectoryToSpecial(ConfigDirectory, ConfigFile);

				if(File.Exists(filename))
					return true;
				else
				{
					new LogConfig(d_logfilename, d_logdatefilename, d_logmaxfilesize, 3, d_logdirectory, d_irclogdirectory, d_irclog);
					Log.Initialize(d_logfilename, ColorBindMode);
					Log.Error("YamlConfig", sLConsole.Config("Text5"));
					Log.Debug("YamlConfig", sLConsole.Config("Text6"));
					var yaml = new YamlStream();
					string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "_" + ConfigFile);

					if(File.Exists(filename2))
						yaml.Load(File.OpenText(filename2));

					try
					{
						var FBImap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("FBI")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["FBI".ToYamlNode()]).Children : NullYMap;
						var nodes = new YamlMappingNode();
						var nodes2 = new YamlMappingNode();
						nodes2.Add("Server",       CreateServerMap((!FBImap.IsNull() && FBImap.ContainsKey("Server")) ? ((YamlMappingNode)FBImap["Server".ToYamlNode()]).Children : NullYMap));

						if((!FBImap.IsNull() && FBImap.ContainsKey("Irc")))
						{
							foreach(var irc in FBImap)
							{
								if(irc.Key.ToString().Contains("Irc"))
									nodes2.Add(irc.Key, CreateIrcMap(((YamlMappingNode)irc.Value).Children));
							}
						}
						else
							nodes2.Add("Irc",      CreateIrcMap(NullYMap));

						nodes2.Add("Log",          CreateLogMap((!FBImap.IsNull() && FBImap.ContainsKey("Log")) ? ((YamlMappingNode)FBImap["Log".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("MySql",        CreateMySqlMap((!FBImap.IsNull() && FBImap.ContainsKey("MySql")) ? ((YamlMappingNode)FBImap["MySql".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("SQLite",       CreateSQLiteMap((!FBImap.IsNull() && FBImap.ContainsKey("SQLite")) ? ((YamlMappingNode)FBImap["SQLite".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Crash",        CreateCrashMap((!FBImap.IsNull() && FBImap.ContainsKey("Crash")) ? ((YamlMappingNode)FBImap["Crash".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Localization", CreateLocalizationMap((!FBImap.IsNull() && FBImap.ContainsKey("Localization")) ? ((YamlMappingNode)FBImap["Localization".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Shutdown",     CreateShutdownMap((!FBImap.IsNull() && FBImap.ContainsKey("Shutdown")) ? ((YamlMappingNode)FBImap["Shutdown".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Clean",        CreateCleanMap((!FBImap.IsNull() && FBImap.ContainsKey("Clean")) ? ((YamlMappingNode)FBImap["Clean".ToYamlNode()]).Children : NullYMap));
						nodes.Add("FBI", nodes2);

						sUtilities.CreateFile(filename);
						var file = new StreamWriter(filename, true) { AutoFlush = true };
						file.Write(nodes.Children.ToString("FBI"));
						file.Close();

						if(File.Exists(filename2))
							File.Delete(filename2);

						Log.Success("YamlConfig", sLConsole.Config("Text7"));
					}
					catch(Exception e)
					{
						Log.Error("YamlConfig", sLConsole.Config("Text13"), e.Message);
						errors = true;
					}
				}
			}
			catch(DirectoryNotFoundException)
			{
				CreateConfig(ConfigDirectory, ConfigFile, ColorBindMode);
			}

			return false;
		}

		private void LogMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string LogFileName = (!nodes.IsNull() && nodes.ContainsKey("FileName")) ? nodes["FileName".ToYamlNode()].ToString() : d_logfilename;
			bool LogDateFileName = (!nodes.IsNull() && nodes.ContainsKey("DateFileName")) ? Convert.ToBoolean(nodes["DateFileName".ToYamlNode()].ToString()) : d_logdatefilename;
			int LogMaxFileSize = (!nodes.IsNull() && nodes.ContainsKey("MaxFileSize")) ? Convert.ToInt32(nodes["MaxFileSize".ToYamlNode()].ToString()) : d_logmaxfilesize;
			int LogLevel = (!nodes.IsNull() && nodes.ContainsKey("LogLevel")) ? Convert.ToInt32(nodes["LogLevel".ToYamlNode()].ToString()) : d_loglevel;
			string LogDirectory = (!nodes.IsNull() && nodes.ContainsKey("LogDirectory")) ? nodes["LogDirectory".ToYamlNode()].ToString() : d_logdirectory;
			string IrcLogDirectory = (!nodes.IsNull() && nodes.ContainsKey("IrcLogDirectory")) ? nodes["IrcLogDirectory".ToYamlNode()].ToString() : d_irclogdirectory;
			bool IrcLog = (!nodes.IsNull() && nodes.ContainsKey("IrcLog")) ? Convert.ToBoolean(nodes["IrcLog".ToYamlNode()].ToString()) : d_irclog;

			new LogConfig(LogFileName, LogDateFileName, LogMaxFileSize, LogLevel, sUtilities.GetSpecialDirectory(LogDirectory), sUtilities.GetSpecialDirectory(IrcLogDirectory), IrcLog);
		}

		private void ServerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool ServerEnabled = (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? Convert.ToBoolean(nodes["Enabled".ToYamlNode()].ToString()) : d_serverenabled;
			int ServerPort = (!nodes.IsNull() && nodes.ContainsKey("Port")) ? Convert.ToInt32(nodes["Port".ToYamlNode()].ToString()) : d_serverport;
			string ServerPassword = (!nodes.IsNull() && nodes.ContainsKey("Password")) ? nodes["Password".ToYamlNode()].ToString() : d_serverpassword;

			new ServerConfig(ServerEnabled, ServerPort, ServerPassword);
		}

		private void IrcMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			int MessageSending = d_messagesending;
			
			if(!nodes.IsNull() && nodes.ContainsKey("Wait"))
			{
				var node2 = ((YamlMappingNode)nodes["Wait".ToYamlNode()]).Children;
				MessageSending = (!node2.IsNull() && node2.ContainsKey("MessageSending")) ? Convert.ToInt32(node2["MessageSending".ToYamlNode()].ToString()) : d_messagesending;
			}
			
			string MessageType = (!nodes.IsNull() && nodes.ContainsKey("MessageType")) ? nodes["MessageType".ToYamlNode()].ToString() : d_messagetype;


			new IRCConfig(MessageSending, MessageType);
		}

		private void MySqlMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? Convert.ToBoolean(nodes["Enabled".ToYamlNode()].ToString()) : d_mysqlenabled;
			string Host = (!nodes.IsNull() && nodes.ContainsKey("Host")) ? nodes["Host".ToYamlNode()].ToString() : d_mysqlhost;
			string User = (!nodes.IsNull() && nodes.ContainsKey("User")) ? nodes["User".ToYamlNode()].ToString() : d_mysqluser;
			string Password = (!nodes.IsNull() && nodes.ContainsKey("Password")) ? nodes["Password".ToYamlNode()].ToString() : d_mysqlpassword;
			string Database = (!nodes.IsNull() && nodes.ContainsKey("Database")) ? nodes["Database".ToYamlNode()].ToString() : d_mysqldatabase;
			string Charset = (!nodes.IsNull() && nodes.ContainsKey("Charset")) ? nodes["Charset".ToYamlNode()].ToString() : d_mysqlcharset;

			new MySqlConfig(Enabled, Host, User, Password, Database, Charset);
		}

		private void SQLiteMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? Convert.ToBoolean(nodes["Enabled".ToYamlNode()].ToString()) : d_sqliteenabled;
			string FileName = (!nodes.IsNull() && nodes.ContainsKey("FileName")) ? nodes["FileName".ToYamlNode()].ToString() : d_sqlitefilename;

			new SQLiteConfig(Enabled, sUtilities.GetSpecialDirectory(FileName));
		}

		private void CrashMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string Directory = (!nodes.IsNull() && nodes.ContainsKey("Directory")) ? nodes["Directory".ToYamlNode()].ToString() : d_crashdirectory;

			new CrashConfig(sUtilities.GetSpecialDirectory(Directory));
		}

		private void LocalizationMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string Locale = (!nodes.IsNull() && nodes.ContainsKey("Locale")) ? nodes["Locale".ToYamlNode()].ToString() : d_locale;

			new LocalizationConfig(Locale);
		}

		private void ShutdownMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			int MaxMemory = (!nodes.IsNull() && nodes.ContainsKey("MaxMemory")) ? Convert.ToInt32(nodes["MaxMemory".ToYamlNode()].ToString()) : d_shutdownmaxmemory;

			new ShutdownConfig(MaxMemory);
		}

		private void CleanMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Config = (!nodes.IsNull() && nodes.ContainsKey("Config")) ? Convert.ToBoolean(nodes["Config".ToYamlNode()].ToString()) : d_cleanconfig;
			bool Database = (!nodes.IsNull() && nodes.ContainsKey("Database")) ? Convert.ToBoolean(nodes["Database".ToYamlNode()].ToString()) : d_cleandatabase;
			new CleanConfig(Config, Database);
		}

		private YamlMappingNode CreateServerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Enabled",  (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? nodes["Enabled".ToYamlNode()].ToString() : d_serverenabled.ToString());
			map.Add("Port",     (!nodes.IsNull() && nodes.ContainsKey("Port")) ? nodes["Port".ToYamlNode()].ToString() : d_serverport.ToString());
			map.Add("Password", (!nodes.IsNull() && nodes.ContainsKey("Password")) ? nodes["Password".ToYamlNode()].ToString() : d_serverpassword);
			return map;
		}

		private YamlMappingNode CreateLogMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("FileName",        (!nodes.IsNull() && nodes.ContainsKey("FileName")) ? nodes["FileName".ToYamlNode()].ToString() : d_logfilename);
			map.Add("DateFileName",    (!nodes.IsNull() && nodes.ContainsKey("DateFileName")) ? nodes["DateFileName".ToYamlNode()].ToString() : d_logdatefilename.ToString());
			map.Add("MaxFileSize",     (!nodes.IsNull() && nodes.ContainsKey("MaxFileSize")) ? nodes["MaxFileSize".ToYamlNode()].ToString() : d_logmaxfilesize.ToString());
			map.Add("LogLevel",        (!nodes.IsNull() && nodes.ContainsKey("LogLevel")) ? nodes["LogLevel".ToYamlNode()].ToString() : d_loglevel.ToString());
			map.Add("LogDirectory",    (!nodes.IsNull() && nodes.ContainsKey("LogDirectory")) ? nodes["LogDirectory".ToYamlNode()].ToString() : d_logdirectory);
			map.Add("IrcLogDirectory", (!nodes.IsNull() && nodes.ContainsKey("IrcLogDirectory")) ? nodes["IrcLogDirectory".ToYamlNode()].ToString() : d_irclogdirectory);
			map.Add("IrcLog",          (!nodes.IsNull() && nodes.ContainsKey("IrcLog")) ? nodes["IrcLog".ToYamlNode()].ToString() : d_irclog.ToString());
			return map;
		}

		private YamlMappingNode CreateIrcMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			var map2 = new YamlMappingNode();

			if(!nodes.IsNull() && nodes.ContainsKey("Wait"))
			{
				var node2 = ((YamlMappingNode)nodes["Wait".ToYamlNode()]).Children;
				map2.Add("MessageSending", (!node2.IsNull() && node2.ContainsKey("MessageSending")) ? node2["MessageSending".ToYamlNode()].ToString() : d_messagesending.ToString());
			}
			else
				map2.Add("MessageSending", d_messagesending.ToString());

			map.Add("Wait", map2);
			map.Add("MessageType", (!nodes.IsNull() && nodes.ContainsKey("MessageType")) ? nodes["MessageType".ToYamlNode()].ToString() : d_messagetype);
			return map;
		}

		private YamlMappingNode CreateMySqlMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Enabled",  (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? nodes["Enabled".ToYamlNode()].ToString() : d_mysqlenabled.ToString());
			map.Add("Host",     (!nodes.IsNull() && nodes.ContainsKey("Host")) ? nodes["Host".ToYamlNode()].ToString() : d_mysqlhost);
			map.Add("User",     (!nodes.IsNull() && nodes.ContainsKey("User")) ? nodes["User".ToYamlNode()].ToString() : d_mysqluser);
			map.Add("Password", (!nodes.IsNull() && nodes.ContainsKey("Password")) ? nodes["Password".ToYamlNode()].ToString() : d_mysqlpassword);
			map.Add("Database", (!nodes.IsNull() && nodes.ContainsKey("Database")) ? nodes["Database".ToYamlNode()].ToString() : d_mysqldatabase);
			map.Add("Charset",  (!nodes.IsNull() && nodes.ContainsKey("Charset")) ? nodes["Charset".ToYamlNode()].ToString() : d_mysqlcharset);
			return map;
		}

		private YamlMappingNode CreateSQLiteMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Enabled",  (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? nodes["Enabled".ToYamlNode()].ToString() : d_sqliteenabled.ToString());
			map.Add("FileName", (!nodes.IsNull() && nodes.ContainsKey("FileName")) ? nodes["FileName".ToYamlNode()].ToString() : d_sqlitefilename);
			return map;
		}

		private YamlMappingNode CreateCrashMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Directory", (!nodes.IsNull() && nodes.ContainsKey("Directory")) ? nodes["Directory".ToYamlNode()].ToString() : d_crashdirectory);
			return map;
		}

		private YamlMappingNode CreateLocalizationMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Locale", (!nodes.IsNull() && nodes.ContainsKey("Locale")) ? nodes["Locale".ToYamlNode()].ToString() : d_locale);
			return map;
		}

		private YamlMappingNode CreateShutdownMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("MaxMemory", (!nodes.IsNull() && nodes.ContainsKey("MaxMemory")) ? nodes["MaxMemory".ToYamlNode()].ToString() : d_shutdownmaxmemory.ToString());
			return map;
		}

		private YamlMappingNode CreateCleanMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Config",   (!nodes.IsNull() && nodes.ContainsKey("Config")) ? nodes["Config".ToYamlNode()].ToString() : d_cleanconfig.ToString());
			map.Add("Database", (!nodes.IsNull() && nodes.ContainsKey("Database")) ? nodes["Database".ToYamlNode()].ToString() : d_cleandatabase.ToString());
			return map;
		}
	}
}