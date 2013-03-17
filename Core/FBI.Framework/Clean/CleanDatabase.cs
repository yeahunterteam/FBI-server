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
using System.Collections.Generic;
using FBI.Irc;
using FBI.Framework.Config;
using FBI.Framework.Extensions;
using FBI.Framework.Localization;

namespace FBI.Framework.Clean
{
	public sealed class CleanDatabase
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private bool _clean;
		public bool IsClean() { return _clean; }

		public CleanDatabase()
		{
			try
			{
				Log.Notice("CleanDatabase", sLConsole.CleanDatabase("Text"));
				if(!FBI.Framework.Config.CleanConfig.Database)
				{
					_clean = true;
					return;
				}

				CleanCoreTable();
			}
			catch(Exception e)
			{
				Log.Error("CleanDatabase", sLConsole.Exception("Error"), e.Message);
				_clean = false;
			}

			_clean = true;
		}

		public void CleanTable(string table)
		{
			Log.Debug("CleanDatabase", sLConsole.CleanDatabase("Text2"), table);

			var db = FBIBase.DManager.Query("SELECT ServerName FROM {0} GROUP BY ServerName", table);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["ServerName"].ToString();

					if(!ServerList.List.ContainsKey(name))
					{
						FBIBase.DManager.Delete(table, string.Format("ServerName = '{0}'", name));
						Log.Debug("CleanDatabase", sLConsole.CleanDatabase("Text3"), name, table);
					}
				}
			}

			Log.Debug("CleanDatabase", sLConsole.CleanDatabase("Text4"), table);
		}

		private void CleanCoreTable()
		{
			Log.Notice("CleanDatabase", sLConsole.CleanDatabase("Text5"));
			CleanTable("channels");
			CleanTable("FBI");
			Log.Notice("CleanDatabase", sLConsole.CleanDatabase("Text6"));
		}
	}
}