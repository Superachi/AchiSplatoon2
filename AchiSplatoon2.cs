using AchiSplatoon2.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static AchiSplatoon2.Helpers.NetHelper;

namespace AchiSplatoon2
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class AchiSplatoon2 : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte msgType = reader.ReadByte();

            if (IsThisAClient() && EnableNetDebug)
            {
                Main.NewText($"({DateTime.Now}) Packet of type ({msgType}), from user ({whoAmI}) incoming...");
                Logger.Info($"({DateTime.Now}) Packet of type ({msgType}), from user ({whoAmI}) incoming...");
            }

            switch (msgType)
            {
                case (int)PacketType.TestRequest:
                    ReceiveRequestTestPacket(reader, whoAmI);
                    Logger.Info($"Received test request packet");
                    break;
                case (int)PacketType.TestResponse:
                    ReceiveResponseTestPacket(reader, whoAmI);
                    Logger.Info($"Received test response packet");
                    break;
                case (int)PacketType.PublicMessage:
                    string message = ReceiveMessage(reader, whoAmI);
                    break;
                case (int)PacketType.SpecialReadyDust:
                    ReceivePlayerIsSpecialReady(reader, whoAmI);
                    break;
                default:
                    Logger.WarnFormat("MyMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}
