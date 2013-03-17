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
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;
using FBI.Framework;
using FBI.Framework.Clean;
using FBI.Framework.Config;
using FBI.Framework.Network;
using FBI.Framework.Extensions;
using FBI.Framework.Localization;

namespace FBI.Client
{
	class MainClass
	{
		//private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly CrashDumper sCrashDumper = Singleton<CrashDumper>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private static readonly Windows sWindows = Singleton<Windows>.Instance;
		private static readonly Linux sLinux = Singleton<Linux>.Instance;
		private static readonly Packets sPackets = new Packets();
		private static ClientSocket sClientSocket;

		/// <summary>
		///     A Main függvény. Itt indul el a program.
		/// </summary>
		private static void Main(string[] args)
		{
			sRuntime.SetProcessName("Client");
			System.Console.BackgroundColor = ConsoleColor.Black;
			System.Console.ForegroundColor = ConsoleColor.Gray;
			string host = "127.0.0.1";
			int port = 35220;
			string password = "FBI";
			string opcode = string.Empty;

			// Adatok melyek a kommitok kiírásához kellenek.
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
				else if(arg.Contains("--opcode="))
				{
					if(arg.Substring(arg.IndexOf("=")+1) != string.Empty)
						opcode = arg.Substring(arg.IndexOf("=")+1);
					
					continue;
				}
				else if(arg.Contains("--message="))
				{
					string mess = args.SplitToString(FBIBase.Space);
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
			//System.Console.WriteLine(sLConsole.MainText("StartText2-2"), Consts.FBIWebsite);
			//System.Console.WriteLine(sLConsole.MainText("StartText2-3"), Consts.FBIProgrammedBy);
			//System.Console.WriteLine(sLConsole.MainText("StartText2-4"), Consts.FBIDevelopers);
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

			sClientSocket = new ClientSocket(host, port, password);
			sClientSocket.Socket();
			Thread.Sleep(500);

			if(opcode != string.Empty)
			{
				switch(opcode)
				{
				case "0x08":
					sPackets.Commit(project, refname, rev, author, url, channels, ircserver, message);
					break;
				case "0x09":
					sPackets.AddChannel(channels, ircserver);
					break;
				case "0x10":
					sPackets.RemoveChannel(channels, ircserver);
					break;
				case "0x11":
					break;
				case "0x12":
					break;
				}
			}
			else
			{
				// Commit
				sPackets.Commit(project, refname, rev, author, url, channels, ircserver, message);
			}

			Exit();
		}

		/// <summary>
		///     Segítséget nyújt a kapcsolokhoz.
		/// </summary>
		private static void Help()
		{
			System.Console.WriteLine("[Client] Version: {0}", sUtilities.GetVersion());
			System.Console.WriteLine("Options:");
			System.Console.WriteLine("\t-h, --help\t\t\tShow help");
		}
 
		public static void Shutdown(Exception eventArgs = null)
		{
			/*var packet = new FBIPacket();
			packet.Write<int>((int)Opcode.SMSG_CLOSE_CONNECTION);
			packet.Write<int>((int)0);
			ClientSocket.SendPacketToSCS(packet);*/

			if(!eventArgs.IsNull())
			{
				//Log.Error("Main", sLConsole.MainText("StartText4"), eventArgs);
				Console.WriteLine("Crash: {0}", eventArgs.Message);
				sCrashDumper.CreateCrashDump(eventArgs);
			}

			Process.GetCurrentProcess().Kill();
		}

		private static void Exit()
		{
			// Close
			sPackets.Close();

			Thread.Sleep(1000);
			sClientSocket.Close();
			sClientSocket.Dispose();
			Environment.Exit(0);
		}
	}
}