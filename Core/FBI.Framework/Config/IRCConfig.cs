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
	public sealed class IRCConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static int MessageSending { get; private set; }
		public static string MessageType { get; private set; }

		public IRCConfig(int messagesending, string messagetype)
		{
			MessageSending        = messagesending;
			MessageType           = messagetype;
			Log.Notice("IRCConfig", sLConsole.IRCConfig("Text"));
		}
	}
}