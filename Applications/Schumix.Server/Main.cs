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
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;
using Schumix.Framework;
using Schumix.Framework.Clean;
using Schumix.Framework.Client;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Server
{
	class MainClass
	{
		//private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly CrashDumper sCrashDumper = Singleton<CrashDumper>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private static readonly Windows sWindows = Singleton<Windows>.Instance;
		private static readonly Linux sLinux = Singleton<Linux>.Instance;

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		private static void Main(string[] args)
		{
			sRuntime.SetProcessName("Cliens");
			System.Console.BackgroundColor = ConsoleColor.Black;
			System.Console.ForegroundColor = ConsoleColor.Gray;
			string host = "127.0.0.1";
			int port = 35220;
			string password = "schumix";
			string message = string.Empty;
			string project = string.Empty;
			string refname = string.Empty;
			string rev = string.Empty;
			string author = string.Empty;
			string url = string.Empty;
			string channels = string.Empty;
			string ircserver = string.Empty;

			for(int i = 0; i < args.Length; i++)
			{
				string arg = args[i];

				if(arg == "-h" || arg == "--help")
				{
					Help();
					return;
				}
				else if(arg.Contains("--host="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						host = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--port="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						port = Convert.ToInt32(arg.Substring(arg.IndexOf("=")+1));
					
					continue;
				}
				else if(arg.Contains("--password="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						password = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--project="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						project = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--author="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						author = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--url="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						url = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--refname="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						refname = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--rev="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						rev = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--channels="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						channels = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--ircserver="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						ircserver = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--message="))
				{
					string mess = args.SplitToString(SchumixBase.Space);
					if(mess.Substring(mess.IndexOf("--message=") + "--message=".Length) != string.Empty)
						message = mess.Substring(mess.IndexOf("--message=") + "--message=".Length);

					continue;
				}
			}

			//System.Console.Title = "FBI Test Cliens";
			//System.Console.ForegroundColor = ConsoleColor.Blue;
			//System.Console.WriteLine("[Cliens]");
			//System.Console.WriteLine(sLConsole.MainText("StartText"));
			//System.Console.WriteLine(sLConsole.MainText("StartText2"), sUtilities.GetVersion());
			//System.Console.WriteLine(sLConsole.MainText("StartText2-2"), Consts.SchumixWebsite);
			//System.Console.WriteLine(sLConsole.MainText("StartText2-3"), Consts.SchumixProgrammedBy);
			//System.Console.WriteLine(sLConsole.MainText("StartText2-4"), Consts.SchumixDevelopers);
			//System.Console.WriteLine("================================================================================"); // 80
			//System.Console.ForegroundColor = ConsoleColor.Gray;
			//System.Console.WriteLine();

			//Log.Notice("Main", sLConsole.MainText("StartText3"));

			if(sUtilities.GetPlatformType() == PlatformType.Windows)
				sWindows.Init();
			else if(sUtilities.GetPlatformType() == PlatformType.Linux)
				sLinux.Init();

			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				Shutdown(eventArgs.ExceptionObject as Exception);
			};

			var listener = new ClientSocket(host, port, password);
			listener.Socket();
			Thread.Sleep(500);

			var packet = new SchumixPacket();
			packet.Write<int>((int)Opcode.CMSG_REQUEST_TEST);
			packet.Write<string>(project);
			packet.Write<string>(refname);
			packet.Write<string>(rev);
			packet.Write<string>(author);
			packet.Write<string>(url);
			packet.Write<string>(channels);
			packet.Write<string>(ircserver);
			packet.Write<string>(message);
			ClientSocket.SendPacketToSCS(packet);
			
			// Close
			var packet2 = new SchumixPacket();
			packet2.Write<int>((int)Opcode.CMSG_CLOSE_CONNECTION);
			packet2.Write<string>(SchumixBase.GetGuid().ToString());
			ClientSocket.SendPacketToSCS(packet2);
			Thread.Sleep(1000);
			listener.Close();
			listener.Dispose();
			Environment.Exit(0);
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void Help()
		{
			System.Console.WriteLine("[Cliens] Version: {0}", sUtilities.GetVersion());
			System.Console.WriteLine("Options:");
			System.Console.WriteLine("\t-h, --help\t\t\tShow help");
		}
 
		public static void Shutdown(Exception eventArgs = null)
		{
			if(!eventArgs.IsNull())
			{
				//Log.Error("Main", sLConsole.MainText("StartText4"), eventArgs);
				Console.WriteLine("Crash: {0}", eventArgs.Message);
				sCrashDumper.CreateCrashDump(eventArgs);
			}

			Process.GetCurrentProcess().Kill();
		}
	}
}