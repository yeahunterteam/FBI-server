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
using Schumix.Framework.Network;

namespace Schumix.Client
{
	class Packets
	{
		public Packets()
		{
		}

		public void Commit(string project, string refname, string rev, string author, string url, string channels, string ircserver, string message)
		{
			var packet = new SchumixPacket();
			packet.Write<int>((int)Opcode.CMSG_REQUEST_COMMIT);
			packet.Write<string>(project);
			packet.Write<string>(refname);
			packet.Write<string>(rev);
			packet.Write<string>(author);
			packet.Write<string>(url);
			packet.Write<string>(channels);
			packet.Write<string>(ircserver);
			packet.Write<string>(message);
			ClientSocket.SendPacketToSCS(packet);
		}

		public void Close()
		{
			var packet = new SchumixPacket();
			packet.Write<int>((int)Opcode.CMSG_CLOSE_CONNECTION);
			packet.Write<string>(SchumixBase.GetGuid().ToString());
			ClientSocket.SendPacketToSCS(packet);
		}
	}
}