﻿#region

using UlteriusServer.Api.Network.Messages;
using UlteriusServer.Api.Network.Models;
using UlteriusServer.Api.Services.Network;
using UlteriusServer.WebSocketAPI.Authentication;
using static UlteriusServer.Api.Network.PacketManager;

#endregion

namespace UlteriusServer.Api.Network.PacketHandlers
{
    public class NetworkPacketHandler : PacketHandler
    {
        private MessageBuilder _builder;
        private AuthClient _client;
        private Packet _packet;


        public void GetNetworkInformation()
        {
            if (string.IsNullOrEmpty(NetworkInformation.PublicIp))
            {
                NetworkInformation.PublicIp = NetworkService.GetPublicIp();
                NetworkInformation.NetworkComputers = NetworkService.ConnectedDevices();
                NetworkInformation.MacAddress = NetworkService.GetMacAddress().ToString();
                NetworkInformation.InternalIp = NetworkService.GetIpAddress().ToString();
            }
            _builder.WriteMessage(NetworkInformation.ToObject());
        }

        public override void HandlePacket(Packet packet)
        {
            _client = packet.AuthClient;
            _packet = packet;
            _builder = new MessageBuilder(_client, _packet.EndPoint, _packet.SyncKey);
            switch (_packet.PacketType)
            {
                case PacketTypes.RequestNetworkInformation:
                    GetNetworkInformation();
                    break;
            }
        }
    }
}