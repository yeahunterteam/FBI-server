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
using System.Diagnostics;
using Schumix.API;
using Schumix.API.Irc;
using Schumix.API.Functions;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public abstract partial class MessageHandler
	{
		protected void HandlePrivmsg(IRCMessage sIRCMessage)
		{
			if(ConsoleLog.CLog)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[{0}] <{1}> {2}", sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			if(sIRCMessage.Args.Contains(((char)1).ToString()))
			{
				string args = sIRCMessage.Args;
				args = args.Remove(0, 1, (char)1);
				args = args.Substring(0, args.IndexOf((char)1));

				if(args.Length > 6 && args.Substring(0, 6) == "ACTION")
				{
					args = args.Remove(0, 7);
					LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, string.Format(sLConsole.MessageHandler("Text25"), args));
				}
			}
			else
				LogToFile(sIRCMessage.Channel, sIRCMessage.Nick, sIRCMessage.Args);
		}
	}
}