using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode;
using System;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static AchiSplatoon2.Helpers.NetHelper;

namespace AchiSplatoon2
{
    enum PacketHandlerType : byte
    {
        Generic,
        Player
    }

    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class AchiSplatoon2 : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte handlerType = reader.ReadByte();
            switch (handlerType)
            {
                case (int)PacketHandlerType.Generic:
                    Logger.Info("Received generic packet, handling it via GenericPacketHandler...");
                    new GenericPacketHandler(reader, whoAmI, this.Logger).HandlePacket();
                    break;
                case (int)PacketHandlerType.Player:
                    Logger.Info("Received player-related packet, handling it via ModPlayerPacketHandler...");
                    new ModPlayerPacketHandler(reader, whoAmI, this.Logger).HandlePacket();
                    break;
                default:
                    Logger.WarnFormat("Unknown Packet Handler type: {0}", handlerType);
                    break;
            }
        }
    }
}
