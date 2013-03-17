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
using FBI.Framework.Localization;

namespace FBI.Framework.Config
{
	public sealed class MySqlConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static bool Enabled { get; private set; }
		public static string Host { get; private set; }
		public static string User { get; private set; }
		public static string Password { get; private set; }
		public static string Database { get; private set; }
		public static string Charset { get; private set; }

		public MySqlConfig(bool enabled, string host, string user, string password, string database, string charset)
		{
			Enabled  = enabled;
			Host     = host;
			User     = user;
			Password = password;
			Database = database;
			Charset  = charset;
			Log.Notice("MySqlConfig", sLConsole.MySqlConfig("Text"));
		}
	}
}