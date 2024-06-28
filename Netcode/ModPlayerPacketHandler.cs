using AchiSplatoon2.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static AchiSplatoon2.Helpers.NetHelper;

namespace AchiSplatoon2.Netcode
{
    enum PlayerPacketType : byte
    {
        SpecialReady,
        UpdateInkColor
    }

    internal class ModPlayerPacketHandler
    {
        private readonly BinaryReader _reader;
        private readonly int _whoAmI;
        private readonly ILog _logger;

        public ModPlayerPacketHandler(BinaryReader reader, int whoAmI, ILog logger)
        {
            _reader = reader;
            _whoAmI = whoAmI;
            _logger = logger;
        }

        public void HandlePacket()
        {
            byte msgType = _reader.ReadByte();

            switch (msgType)
            {
                case (int)PlayerPacketType.SpecialReady:
                    ReceivePlayerIsSpecialReady(_reader, _whoAmI);
                    break;
                default:
                    _logger.WarnFormat("MyMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }

        public static void SendPlayerIsSpecialReady(int fromWho, bool isSpecialReady)
        {
            // Prepare data
            Player player = Main.LocalPlayer;
            ModPacket packet = GetNewPacket();

            // Write
            WritePacketHandlerType(packet, (int)PacketHandlerType.Player);
            WritePacketType(packet, (int)PlayerPacketType.SpecialReady);
            WritePacketFromWhoID(packet, fromWho);
            packet.Write(isSpecialReady);

            // Send
            SendPacket(packet, toClient: -1, ignoreClient: fromWho);
        }

        public static void ReceivePlayerIsSpecialReady(BinaryReader reader, int fromWho)
        {
            // Read data
            fromWho = reader.ReadInt32();
            bool isSpecialReady = reader.ReadBoolean();

            // Respond
            if (IsThisTheServer())
            {
                // Forward
                SendPlayerIsSpecialReady(fromWho, isSpecialReady);
            }
            else
            {
                var modPlayer = GetModPlayerFromPacket(fromWho);
                modPlayer.SpecialReady = isSpecialReady;
            }
        }
    }
}
