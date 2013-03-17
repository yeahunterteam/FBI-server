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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FBI.Irc;
using FBI.Framework.Extensions;

namespace FBI.Framework
{
	public sealed class ServerList
	{
		private static readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		public static Dictionary<string, IrcServer> List { get; private set; }

		static ServerList()
		{
			List = new Dictionary<string, IrcServer>();
		}

		public static void NewServer(string ServerName)
		{
			ServerName = ServerName.ToLower();
			var db = FBIBase.DManager.QueryFirstRow("SELECT * FROM servers WHERE ServerName = '{0}'", ServerName);
			if(!db.IsNull())
			{
				List.Add(ServerName, new IrcServer(ServerName));
				sIrcBase.NewServer(ServerName, List[ServerName].ServerId(), List[ServerName].Server(), List[ServerName].Port());
				
				Task.Factory.StartNew(() =>
				{
					int i = 0;
					sIrcBase.Connect(ServerName);
						
					while(!sIrcBase.Networks[ServerName].Online)
					{
						if(i >= 30)
							break;
							
						i++;
						Thread.Sleep(1000);
					}
				});
			}
			else
				Log.Error("ServerList", "Nincs ilyen szerver!");
		}

		public static void RemoveServer(string ServerName)
		{
			ServerName = ServerName.ToLower();

			if(List.ContainsKey(ServerName))
			{
				sIrcBase.Networks[ServerName].DisConnect();
				List.Remove(ServerName);
			}
			else
				Log.Error("ServerList", "Nincs ilyen szerver!");
		}
	}
}