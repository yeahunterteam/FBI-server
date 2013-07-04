/*
 * This file is part of FBI.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using FBI.Framework;
using FBI.Framework.Irc;
using FBI.Framework.Config;
using FBI.Framework.Network;
using FBI.Framework.Extensions;
using FBI.Framework.Localization;

namespace FBI.Client
{
	/// <summary>
	/// Packet handler used by the client.
	/// </summary>
	class ClientPacketHandler
	{
		/// <summary>
		/// Occurs when auth is denied.
		/// </summary>
		public event ClientPacketHandlerDelegate OnAuthDenied;
		/// <summary>
		/// Occurs when auth is approved.
		/// </summary>
		public event ClientPacketHandlerDelegate OnAuthApproved;
		public event ClientPacketHandlerDelegate OnCloseConnection;
		private ClientPacketHandler() {}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
			OnAuthApproved    += AuthApprovedHandler;
			OnAuthDenied      += AuthDeniedHandler;
			OnCloseConnection += CloseHandler;
		}

		/// <summary>
		/// Handles the packet.
		/// </summary>
		/// <param name='packet'>
		/// Packet.
		/// </param>
		/// <param name='client'>
		/// Client.
		/// </param>
		public void HandlePacket(FBIPacket packet, TcpClient client)
		{
			var hst = client.Client.RemoteEndPoint.ToString().Split(FBIBase.Colon)[0];
			var packetid = packet.Read<int>();
			//Log.Debug("PacketHandler", sLConsole.ClientPacketHandler("Text"), packetid, client.Client.RemoteEndPoint);

			if(packetid == (int)Opcode.SMSG_AUTH_DENIED)
				OnAuthDenied(packet, hst);
			else if(packetid == (int)Opcode.SMSG_AUTH_APPROVED)
				OnAuthApproved(packet, hst);
			else if(packetid == (int)Opcode.SMSG_CLOSE_CONNECTION)
				OnCloseConnection(packet, hst);
		}

		/// <summary>
		/// The auth denied packet handler. (SMSG_AUTH_DENIED)
		/// </summary>
		/// <param name='pck'>
		/// Packet.
		/// </param>
		/// <param name='hst'>
		/// Host.
		/// </param>
		public void AuthDeniedHandler(FBIPacket pck, string hst)
		{
			//Log.Error("FBIServer", sLConsole.ClientPacketHandler("Text2"));
			//Log.Warning("CloseHandler", sLConsole.ClientPacketHandler("Text3"));
			//Log.Warning("CloseHandler", sLConsole.ClientPacketHandler("Text4"));
			Thread.Sleep(1000);
			Environment.Exit(1);
		}

		/// <summary>
		/// The auth approved packet handler. (SMSG_AUTH_APPROVED)
		/// </summary>
		/// <param name='pck'>
		/// Packet.
		/// </param>
		/// <param name='hst'>
		/// Host.
		/// </param>
		public void AuthApprovedHandler(FBIPacket pck, string hst)
		{
			//Log.Success("FBIServer", sLConsole.ClientPacketHandler("Text5"));
		}

		private void CloseHandler(FBIPacket pck, string hst)
		{
			//if(FBIBase.ExitStatus)
			//	return;

			//Log.Warning("CloseHandler", sLConsole.ClientPacketHandler("Text3"));
			//Log.Warning("CloseHandler", sLConsole.ClientPacketHandler("Text4"));
			//FBIBase.Quit();

			//foreach(var nw in INetwork.WriterList)
			//{
			//	if(!nw.Value.IsNull())
			//		nw.Value.WriteLine("QUIT :Server killed.");
			//}

			Thread.Sleep(1000);
			Environment.Exit(0);
		}
	}
}