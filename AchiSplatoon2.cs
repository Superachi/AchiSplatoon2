using AchiSplatoon2.Helpers;
using System;
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
            Logger.Info($"({DateTime.Now}) Packet incoming...");

            byte msgType = reader.ReadByte();
            switch (msgType)
            {
                case 0:
                    ReceiveRequestTestPacket(reader, whoAmI);
                    Logger.Info($"Received test request packet");
                    break;
                case 1:
                    ReceiveResponseTestPacket(reader, whoAmI);
                    Logger.Info($"Received test response packet");
                    break;
                case 2:
                    string message = ReceiveMessage(reader, whoAmI);
                    Logger.Info(message);
                    break;
                default:
                    Logger.WarnFormat("MyMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}
