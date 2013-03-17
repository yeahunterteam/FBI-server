/*
 * This file is part of FBI.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 FBI Team <http://FBI.eu/>
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
	public sealed class CleanConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static bool Config { get; private set; }
		public static bool Database { get; private set; }

		public CleanConfig(bool config, bool database)
		{
			Config = config;
			Database = database;
			Log.Notice("CleanConfig", sLConsole.CleanConfig("Text"));
		}
	}
}