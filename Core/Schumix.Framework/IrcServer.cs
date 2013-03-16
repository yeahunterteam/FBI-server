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
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public class IrcServer
	{
		private readonly object Lock = new object();
		private string _servername;

		public IrcServer(string ServerName)
		{
			_servername = ServerName.ToLower();
		}

		public int ServerId()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT ServerId FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					string s = db["ServerId"].ToString();
					return s.IsNumber() ? s.ToNumber().ToInt() : -1;
				}
				else
					return -1;
			}
		}

		public string ServerName()
		{
			lock(Lock)
			{
				return _servername;
			}
		}

		public string Server()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Server FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
					return db["Server"].ToString();
				else
					return "localhost";
			}
		}

		public int Port()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Port FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					string s = db["Port"].ToString();
					return s.IsNumber() ? s.ToNumber().ToInt() : -1;
				}
				else
					return -1;
			}
		}

		public bool Ssl()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT SslType FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					string s = db["SslType"].ToString();
					return Convert.ToBoolean(s);
				}
				else
					return false;
			}
		}

		public string NickName()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT NickName FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
					return db["NickName"].ToString();
				else
					return "FBI";
			}
		}

		public string NickName2()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT NickName2 FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
					return db["NickName2"].ToString();
				else
					return "_FBI";
			}
		}

		public string NickName3()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT NickName3 FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
					return db["NickName3"].ToString();
				else
					return "__FBI";
			}
		}

		public string UserName()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT UserName FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
					return db["UserName"].ToString();
				else
					return "FBI";
			}
		}

		public string UserInfo()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT UserInfo FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
					return db["UserInfo"].ToString();
				else
					return "FBI IRC Bot";
			}
		}

		public bool UseNickServ()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT UseNickServ FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					string s = db["UseNickServ"].ToString();
					return Convert.ToBoolean(s);
				}
				else
					return false;
			}
		}

		public string NickServPassword()
		{
			lock(Lock)
			{
				var db = SchumixBase.DManager.QueryFirstRow("SELECT NickServPassword FROM servers WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
					return db["NickServPassword"].ToString();
				else
					return string.Empty;
			}
		}

		public bool UseHostServ()
		{
			lock(Lock)
			{
				lock(Lock)
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT UseHostServ FROM servers WHERE ServerName = '{0}'", _servername);
					if(!db.IsNull())
					{
						string s = db["UseHostServ"].ToString();
						return Convert.ToBoolean(s);
					}
					else
						return false;
				}
			}
		}

		public bool HostServEnabled()
		{
			lock(Lock)
			{
				lock(Lock)
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT HostServEnabled FROM servers WHERE ServerName = '{0}'", _servername);
					if(!db.IsNull())
					{
						string s = db["HostServEnabled"].ToString();
						return Convert.ToBoolean(s);
					}
					else
						return false;
				}
			}
		}
	}
}