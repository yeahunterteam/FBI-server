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
using FBI.Api;
using FBI.Irc;
using FBI.Framework;
using FBI.Framework.Config;
using FBI.Framework.Database;
using FBI.Framework.Extensions;
using FBI.Framework.Localization;

namespace FBI
{
	/// <summary>
	///     Fő class. Innen indul a konzol vezérlés és az irc kapcsolat létrehozása.
	/// </summary>
	sealed class FBIBot
	{
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		public static FBIBase sFBIBase { get; private set; }

		/// <summary>
		///     Indulási függvény.
		/// </summary>
		public FBIBot()
		{
			try
			{
				bool e = false;
				string eserver = string.Empty;
				Log.Notice("FBIBot", sLConsole.FBIBot("Text"));
				Log.Debug("FBIBot", sLConsole.FBIBot("Text2"));
				sFBIBase = new FBIBase();

				var db = FBIBase.DManager.Query("SELECT ServerName FROM servers");
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string name = row["ServerName"].ToString();
						ServerList.List.Add(name, new IrcServer(name));
					}
				}
				else
					Log.Warning("FBIBot", "Nem áll rendelkezésre irc szerver amit be lehetne tölteni!");

				foreach(var sn in ServerList.List)
				{
					if(!e)
					{
						eserver = sn.Key;
						e = true;
					}

					sIrcBase.NewServer(sn.Key, sn.Value.ServerId(), sn.Value.Server(), sn.Value.Port());
				}

				Task.Factory.StartNew(() =>
				{
					if(ServerList.List.Count == 1)
					{
						sIrcBase.Connect(eserver);
						return;
					}

					int i = 0;
					foreach(var sn in ServerList.List)
					{
						sIrcBase.Connect(sn.Key);

						while(!sIrcBase.Networks[sn.Key].Online)
						{
							if(i >= 30)
								break;

							i++;
							Thread.Sleep(1000);
						}
					}
				});

				Log.Debug("FBIBot", sLConsole.FBIBot("Text3"));
				new Console.Console(eserver);
			}
			catch(Exception e)
			{
				Log.Error("FBIBot", sLConsole.Exception("Error"), e/*.Message*/);
			}
		}

		/// <summary>
		///     Destruktor.
		/// </summary>
		/// <remarks>
		///     Ha ez lefut, akkor a class leáll.
		/// </remarks>
		~FBIBot()
		{
			Log.Debug("FBIBot", "~FBIBot()");
		}
	}
}