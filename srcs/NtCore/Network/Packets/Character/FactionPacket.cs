﻿using NtCore.API.Enums;

namespace NtCore.Network.Packets.Character
{
    [PacketInfo("fs", PacketType.Recv)]
    public class FactionPacket : Packet
    {
        [PacketIndex(1)]
        public Faction Faction { get; set; }
    }
}