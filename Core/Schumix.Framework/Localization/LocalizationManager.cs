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
using System.Text;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Localization
{
	public sealed class LocalizationManager
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly object WriteLock = new object();
		public string Locale { get; set; }
		private LocalizationManager() {}

		public string GetConsoleWarningText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleWarningMap().ContainsKey(Locale + command) ? SchumixBase.sCacheDB.LocalizedConsoleWarningMap()[Locale + command].Text : sLConsole.Translations("NoFound");
			}
		}

		public string[] GetConsoleWarningTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleWarningMap().ContainsKey(Locale + command) ? Split(SchumixBase.sCacheDB.LocalizedConsoleWarningMap()[Locale + command].Text) : new string[] { sLConsole.Translations("NoFound") };
			}
		}

		public string GetConsoleCommandText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleCommandMap().ContainsKey(Locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedConsoleCommandMap()[Locale + command.ToLower()].Text : sLConsole.Translations("NoFound");
			}
		}

		public string[] GetConsoleCommandTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleCommandMap().ContainsKey(Locale + command.ToLower()) ? Split(SchumixBase.sCacheDB.LocalizedConsoleCommandMap()[Locale + command.ToLower()].Text) : new string[] { sLConsole.Translations("NoFound") };
			}
		}

		public string GetConsoleCommandHelpText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleCommandHelpMap().ContainsKey(Locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedConsoleCommandHelpMap()[Locale + command.ToLower()].Text : sLConsole.Other("NoFoundHelpCommand");
			}
		}

		public string[] GetConsoleCommandHelpTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleCommandHelpMap().ContainsKey(Locale + command.ToLower()) ? Split(SchumixBase.sCacheDB.LocalizedConsoleCommandHelpMap()[Locale + command.ToLower()].Text) : new string[] { sLConsole.Other("NoFoundHelpCommand") };
			}
		}

		private string[] Split(string Text)
		{
			lock(WriteLock)
			{
				return SQLiteConfig.Enabled ? Text.Split(new string[] { @"\n" }, StringSplitOptions.None) : Text.Split(SchumixBase.NewLine);
			}
		}
	}
}