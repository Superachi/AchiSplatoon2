using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataTransferObjects;
using log4net;
using Newtonsoft.Json;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static AchiSplatoon2.Helpers.NetHelper;

namespace AchiSplatoon2.Netcode
{
    enum PlayerPacketType : byte
    {
        SyncModPlayer,
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
            _logger.Info($"Received '{(PlayerPacketType)msgType}' packet.");

            int fromWho = _reader.ReadInt32();

            switch (msgType)
            {
                case (byte)PlayerPacketType.SyncModPlayer:
                    ReceiveSyncPlayer(_reader, fromWho, _logger);
                    break;
                default:
                    _logger.WarnFormat("Unknown Message type: {0}", msgType);
                    break;
            }
        }

        public static void SendModPlayerPacket(PlayerPacketType msgType, int fromWho, string json = "", ILog logger = null)
        {
            if (IsSinglePlayer()) return;

            // Prepare data
            Player player = Main.LocalPlayer;
            ModPacket packet = GetNewPacket();

            WritePacketHandlerType(packet, (int)PacketHandlerType.Player);
            WritePacketType(packet, (int)msgType);
            WritePacketFromWhoID(packet, fromWho);
            packet.Write(json);

            SendPacket(packet, toClient: -1, ignoreClient: fromWho);
        }

        public static void ReceiveSyncPlayer(BinaryReader reader, int fromWho, ILog logger)
        {
            // 'Payload'
            string json = reader.ReadString();
            var incomingDTO = InkWeaponPlayerDTO.Deserialize(json);
            if (incomingDTO == null) return;

            // Respond
            if (IsThisTheServer())
            {
                // Forward
                SendModPlayerPacket(
                    msgType: PlayerPacketType.SyncModPlayer,
                    fromWho: fromWho,
                    json: json,
                    logger: logger);
            }
            else
            {
                var modPlayer = GetModPlayerFromPacket(fromWho);
                incomingDTO.ApplyToModPlayer(modPlayer);
            }
        }
    }
}
