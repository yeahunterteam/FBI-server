/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
 * 
 * Schumix is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Schumix is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Schumix.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace Schumix.Irc
{
	public class IrcServer
	{
		private readonly object Lock = new object();

		public IrcServer()
		{
		}

		public int ServerId()
		{
			lock(Lock)
			{
				return 1;
			}
		}

		public string ServerName()
		{
			lock(Lock)
			{
				return "default";
			}
		}

		public string Server()
		{
			lock(Lock)
			{
				return "irc.yeahunter.hu";
			}
		}

		public int Port()
		{
			lock(Lock)
			{
				return 6667;
			}
		}

		public bool Ssl()
		{
			lock(Lock)
			{
				return false;
			}
		}

		public string NickName()
		{
			lock(Lock)
			{
				return "fbi-teszt";
			}
		}

		public string NickName2()
		{
			lock(Lock)
			{
				return string.Empty;
			}
		}

		public string NickName3()
		{
			lock(Lock)
			{
				return string.Empty;
			}
		}

		public string UserName()
		{
			lock(Lock)
			{
				return "FBI";
			}
		}

		public string UserInfo()
		{
			lock(Lock)
			{
				return "FBI";
			}
		}

		public bool UseNickServ()
		{
			lock(Lock)
			{
				return false;
			}
		}

		public string NickServPassword()
		{
			lock(Lock)
			{
				return string.Empty;
			}
		}

		public bool UseHostServ()
		{
			lock(Lock)
			{
				return false;
			}
		}

		public bool HostServEnabled()
		{
			lock(Lock)
			{
				return false;
			}
		}
	}
}