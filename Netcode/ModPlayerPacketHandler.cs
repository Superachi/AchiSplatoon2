using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Netcode.DataTransferObjects;
using log4net;
using Microsoft.Xna.Framework;
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
        SyncMoveSpeed,
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
                case (byte)PlayerPacketType.SyncMoveSpeed:
                    ReceiveSyncMoveSpeed(_reader, fromWho, _logger);
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

            switch (msgType)
            {
                case PlayerPacketType.SyncModPlayer:
                    SendSyncPlayer(
                        packet: packet,
                        dto: JsonConvert.DeserializeObject<InkWeaponPlayerDTO>(json));
                    break;
                case PlayerPacketType.SyncMoveSpeed:
                    SendSyncMoveSpeed(
                        packet: packet,
                        dto: JsonConvert.DeserializeObject<PlayerMoveSpeedDTO>(json));
                    break;
            }

            SendPacket(packet, toClient: -1, ignoreClient: fromWho);
        }

        public static void SendSyncPlayer(ModPacket packet, InkWeaponPlayerDTO dto)
        {
            packet.Write(dto.SpecialReady);
            packet.WriteRGB(dto.InkColor);
        }

        public static void ReceiveSyncPlayer(BinaryReader reader, int fromWho, ILog logger)
        {
            // 'Payload'
            bool specialReady = reader.ReadBoolean();
            Color colorFromChips = reader.ReadRGB();

            // Respond
            if (IsThisTheServer())
            {
                var dto = new InkWeaponPlayerDTO(specialReady, colorFromChips);
                var json = JsonConvert.SerializeObject(dto);

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
                modPlayer.SpecialReady = specialReady;
                modPlayer.ColorFromChips = colorFromChips;
            }
        }

        public static void SendSyncMoveSpeed(ModPacket packet, PlayerMoveSpeedDTO dto)
        {
            packet.Write((double)dto.moveSpeedMod);
            packet.Write((double)dto.moveAccelMod);
        }

        public static void ReceiveSyncMoveSpeed(BinaryReader reader, int fromWho, ILog logger)
        {
            // 'Payload'
            float moveSpeedMod = (float)reader.ReadDouble();
            float moveAccelMod = (float)reader.ReadDouble();

            // Respond
            if (IsThisTheServer())
            {
                var dto = new PlayerMoveSpeedDTO(moveSpeedMod, moveAccelMod);
                var json = JsonConvert.SerializeObject(dto);

                // Forward
                SendModPlayerPacket(
                    msgType: PlayerPacketType.SyncMoveSpeed,
                    fromWho: fromWho,
                    json: json,
                    logger: logger);
            }
            else
            {
                InkWeaponPlayer modPlayer = GetModPlayerFromPacket(fromWho);
                modPlayer.moveSpeedModifier = moveSpeedMod;
                modPlayer.moveAccelModifier = moveAccelMod;
            }
        }
    }
}
