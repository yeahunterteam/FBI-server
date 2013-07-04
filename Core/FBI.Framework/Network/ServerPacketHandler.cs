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
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FBI.Framework.Irc;
using FBI.Framework;
using FBI.Framework.Config;
using FBI.Framework.Localization;

namespace FBI.Framework.Network
{
	class ServerPacketHandler
	{
		private readonly Dictionary<string, NetworkStream> _HostList = new Dictionary<string, NetworkStream>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Dictionary<string, bool> _AuthList = new Dictionary<string, bool>();
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public Dictionary<string, NetworkStream> HostList { get { return _HostList; } }
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		public event ServerPacketHandlerDelegate OnCloseConnection;
		public event ServerPacketHandlerDelegate OnAuthRequest;
		public event ServerPacketHandlerDelegate OnCommit;
		public event ServerPacketHandlerDelegate OnAddChannel;
		public event ServerPacketHandlerDelegate OnRemoveChannel;
		public event ServerPacketHandlerDelegate OnAddIrcServer;
		public event ServerPacketHandlerDelegate OnRemoveIrcServer;
		private ServerPacketHandler() {}

		public void Init()
		{
			OnAuthRequest      += AuthRequestPacketHandler;
			OnCloseConnection  += CloseHandler;
			OnCommit           += CommitHandler;
			OnAddChannel       += AddChannelHandler;
			OnRemoveChannel    += RemoveChannelHandler;
			OnAddIrcServer     += AddIrcServerHandler;
			OnRemoveIrcServer  += RemoveIrcServerHandler;
		}

		public void HandlePacket(FBIPacket packet, TcpClient client, NetworkStream stream)
		{
			var hst = client.Client.RemoteEndPoint.ToString().Split(FBIBase.Colon)[0];
			int bck = Convert.ToInt32(client.Client.RemoteEndPoint.ToString().Split(FBIBase.Colon)[1]);

			var packetid = packet.Read<int>();
			Log.Debug("PacketHandler", sLConsole.ServerPacketHandler("Text"), packetid, client.Client.RemoteEndPoint);

			if(!_AuthList.ContainsKey(hst + FBIBase.Colon + bck))
			{
				if(packetid != (int)Opcode.CMSG_REQUEST_AUTH)
				{
					var packet2 = new FBIPacket();
					packet2.Write<int>((int)Opcode.SMSG_AUTH_DENIED);
					packet2.Write<int>((int)0);
					SendPacketBack(packet2, stream, hst, bck);
					return;
				}
				else
					_AuthList.Add(hst + FBIBase.Colon + bck, true);
			}

			if(!_HostList.ContainsKey(hst + FBIBase.Colon + bck))
				_HostList.Add(hst + FBIBase.Colon + bck, stream);

			if(packetid == (int)Opcode.CMSG_REQUEST_AUTH)
				OnAuthRequest(packet, stream, hst, bck);
			else if(packetid == (int)Opcode.CMSG_CLOSE_CONNECTION)
				OnCloseConnection(packet, stream, hst, bck);
			else if(packetid == (int)Opcode.CMSG_REQUEST_COMMIT)
				OnCommit(packet, stream, hst, bck);
			else if(packetid == (int)Opcode.CMSG_REQUEST_CHANNEL_ADD)
				OnAddChannel(packet, stream, hst, bck);
			else if(packetid == (int)Opcode.CMSG_REQUEST_CHANNEL_REMOVE)
				OnRemoveChannel(packet, stream, hst, bck);
			else if(packetid == (int)Opcode.CMSG_REQUEST_IRCSERVER_ADD)
				OnAddIrcServer(packet, stream, hst, bck);
			else if(packetid == (int)Opcode.CMSG_REQUEST_IRCSERVER_REMOVE)
				OnRemoveIrcServer(packet, stream, hst, bck);
		}

		private void AuthRequestPacketHandler(FBIPacket pck, NetworkStream stream, string hst, int bck)
		{
			// opcode is already read, DO _NOT_ READ AGAIN
			string guid = pck.Read<string>();
			string hash = pck.Read<string>();

			if(hash != sUtilities.Md5(ServerConfig.Password))
			{
				if(_HostList.ContainsKey(hst + FBIBase.Colon + bck))
					_HostList.Remove(hst + FBIBase.Colon + bck);

				Log.Warning("AuthHandler", sLConsole.ServerPacketHandler("Text2"), guid);
				Log.Debug("Security", sLConsole.ServerPacketHandler("Text3"), hash);
				Log.Notice("AuthHandler", sLConsole.ServerPacketHandler("Text4"), bck);
				var packet = new FBIPacket();
				packet.Write<int>((int)Opcode.SMSG_AUTH_DENIED);
				packet.Write<int>((int)0);
				SendPacketBack(packet, stream, hst, bck);
			}
			else
			{
				Log.Success("AuthHandler", sLConsole.ServerPacketHandler("Text5"), guid);
				Log.Debug("Security", sLConsole.ServerPacketHandler("Text3"), hash);
				Log.Notice("AuthHandler", sLConsole.ServerPacketHandler("Text4"), bck);
				var packet = new FBIPacket();
				packet.Write<int>((int)Opcode.SMSG_AUTH_APPROVED);
				packet.Write<int>((int)1);
				SendPacketBack(packet, stream, hst, bck);
			}
		}

		private void CloseHandler(FBIPacket pck, NetworkStream stream, string hst, int bck)
		{
			if(_HostList.ContainsKey(hst + FBIBase.Colon + bck))
				_HostList.Remove(hst + FBIBase.Colon + bck);

			if(_AuthList.ContainsKey(hst + FBIBase.Colon + bck))
				_AuthList.Remove(hst + FBIBase.Colon + bck);

			string guid = pck.Read<string>();
			Log.Warning("CloseHandler", sLConsole.ServerPacketHandler("Text6"), guid);
			//Log.Notice("CloseHandler", sLConsole.ServerPacketHandler("Text7"));
		}

		private void CommitHandler(FBIPacket pck, NetworkStream stream, string hst, int bck)
		{
			string project = pck.Read<string>();
			string refname = pck.Read<string>();
			string rev = pck.Read<string>().ToLower();
			string author = pck.Read<string>();
			string url = pck.Read<string>();
			string channels = pck.Read<string>().ToLower();
			string ircserver = pck.Read<string>().ToLower();
			string message = pck.Read<string>();

			if(!sIrcBase.Networks.ContainsKey(ircserver))
				return;

			var sSender = sIrcBase.Networks[ircserver].sSender;
			var sChannelInfo = sIrcBase.Networks[ircserver].sChannelInfo;
			var sSendMessage = sIrcBase.Networks[ircserver].sSendMessage;
			project = project.Replace(".git", string.Empty);
			message = message.Length > 400 ? message.Substring(0, 400) + "..." : message;

			foreach(var chan in channels.Split(FBIBase.Comma))
			{
				if(chan.Contains(FBIBase.Colon.ToString()))
				{
					string cname = chan.Substring(0, chan.IndexOf(FBIBase.Colon));
					string cpassword = chan.Substring(chan.IndexOf(FBIBase.Colon)+1);

					if(cpassword.Length > 0)
					{
						if(!sChannelInfo.CList.ContainsKey(cname))
						{
							sSender.Join(cname, cpassword);
							FBIBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", ServerList.List[ircserver].ServerId(), ircserver, cname, cpassword, sLManager.Locale);
							FBIBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", cname, ircserver));
						}
					}
					else
					{
						if(!sChannelInfo.CList.ContainsKey(cname))
						{
							sSender.Join(cname);
							FBIBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", ServerList.List[ircserver].ServerId(), ircserver, cname, string.Empty, sLManager.Locale);
							FBIBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", cname, ircserver));
						}
					}
				}
				else
				{
					if(!sChannelInfo.CList.ContainsKey(chan))
					{
						sSender.Join(chan);
						FBIBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", ServerList.List[ircserver].ServerId(), ircserver, chan, string.Empty, sLManager.Locale);
						FBIBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", chan, ircserver));
					}
				}

				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();

				sSendMessage.SendCMPrivmsg(chan, "[3{0}] {1} pushed new commit to 7{2}: 02{3}", project, author, refname, url);
				sSendMessage.SendCMPrivmsg(chan, "3{0}15/7{1} 10{2} {3}: {4}", project, refname, rev, author, message);
				Thread.Sleep(1000);
			}
		}

		private void AddChannelHandler(FBIPacket pck, NetworkStream stream, string hst, int bck)
		{
			string channels = pck.Read<string>().ToLower();
			string ircserver = pck.Read<string>().ToLower();
			
			if(!sIrcBase.Networks.ContainsKey(ircserver))
				return;
			
			var sSender = sIrcBase.Networks[ircserver].sSender;
			var sChannelInfo = sIrcBase.Networks[ircserver].sChannelInfo;
			
			foreach(var chan in channels.Split(FBIBase.Comma))
			{
				if(chan.Contains(FBIBase.Colon.ToString()))
				{
					string cname = chan.Substring(0, chan.IndexOf(FBIBase.Colon));
					string cpassword = chan.Substring(chan.IndexOf(FBIBase.Colon)+1);
					
					if(cpassword.Length > 0)
					{
						if(!sChannelInfo.CList.ContainsKey(cname))
						{
							sSender.Join(cname, cpassword);
							FBIBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", ServerList.List[ircserver].ServerId(), ircserver, cname, cpassword, sLManager.Locale);
							FBIBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", cname, ircserver));
						}
					}
					else
					{
						if(!sChannelInfo.CList.ContainsKey(cname))
						{
							sSender.Join(cname);
							FBIBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", ServerList.List[ircserver].ServerId(), ircserver, cname, string.Empty, sLManager.Locale);
							FBIBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", cname, ircserver));
						}
					}
				}
				else
				{
					if(!sChannelInfo.CList.ContainsKey(chan))
					{
						sSender.Join(chan);
						FBIBase.DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", ServerList.List[ircserver].ServerId(), ircserver, chan, string.Empty, sLManager.Locale);
						FBIBase.DManager.Update("channels", "Enabled = 'true'", string.Format("Channel = '{0}' And ServerName = '{1}'", chan, ircserver));
					}
				}
				
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
				Thread.Sleep(1000);
			}
		}

		private void RemoveChannelHandler(FBIPacket pck, NetworkStream stream, string hst, int bck)
		{
			string channels = pck.Read<string>().ToLower();
			string ircserver = pck.Read<string>().ToLower();
			
			if(!sIrcBase.Networks.ContainsKey(ircserver))
				return;
			
			var sSender = sIrcBase.Networks[ircserver].sSender;
			var sChannelInfo = sIrcBase.Networks[ircserver].sChannelInfo;
			
			foreach(var chan in channels.Split(FBIBase.Comma))
			{
				if(chan.Contains(FBIBase.Colon.ToString()))
				{
					string cname = chan.Substring(0, chan.IndexOf(FBIBase.Colon));

					if(!sChannelInfo.CList.ContainsKey(cname))
					{
						sSender.Part(cname);
						FBIBase.DManager.Delete("channels", string.Format("Channel = '{0}' And ServerName = '{1}'", cname, ircserver));
					}
				}
				else
				{
					if(!sChannelInfo.CList.ContainsKey(chan))
					{
						sSender.Part(chan);
						FBIBase.DManager.Delete("channels", string.Format("Channel = '{0}' And ServerName = '{1}'", chan, ircserver));
					}
				}
				
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
				Thread.Sleep(1000);
			}
		}

		private void AddIrcServerHandler(FBIPacket pck, NetworkStream stream, string hst, int bck)
		{
			string ircserver = pck.Read<string>().ToLower();
			ServerList.NewServer(ircserver);
		}

		private void RemoveIrcServerHandler(FBIPacket pck, NetworkStream stream, string hst, int bck)
		{
			string ircserver = pck.Read<string>().ToLower();
			ServerList.RemoveServer(ircserver);
		}

		public void SendPacketBack(FBIPacket packet, NetworkStream stream, string hst, int backport)
		{
			Log.Debug("PacketHandler", "SendPacketBack(): host is: " + hst + ", port is: " + backport);

			if(stream.CanWrite)
			{
				var buff = new UTF8Encoding().GetBytes(packet.GetNetMessage());
				stream.Write(buff, 0, buff.Length);
				stream.Flush();
			}
		}
	}
}