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
        WeaponPlayer,
        ColorChipPlayer,
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
                case (byte)PlayerPacketType.WeaponPlayer:
                case (byte)PlayerPacketType.ColorChipPlayer:
                    ReceiveModPlayerDTO((PlayerPacketType)msgType, _reader, fromWho, _logger);
                    break;

                default:
                    _logger.WarnFormat("Unknown Message type: {0}", msgType);
                    break;
            }
        }

        public static void SendModPlayerPacket(PlayerPacketType messageType, int fromWho, string json = "", ILog logger = null)
        {
            if (IsSinglePlayer()) return;

            // Prepare data
            Player player = Main.LocalPlayer;
            ModPacket packet = GetNewPacket();

            WritePacketHandlerType(packet, (int)PacketHandlerType.ModPlayer);
            WritePacketType(packet, (int)messageType);
            WritePacketFromWhoID(packet, fromWho);
            packet.Write(json);

            SendPacket(packet, toClient: -1, ignoreClient: fromWho);
        }

        public static void ReceiveModPlayerDTO(PlayerPacketType messageType, BinaryReader reader, int fromWho, ILog logger)
        {
            // 'Payload'
            string json = reader.ReadString();

            BaseDTO? incomingDTO = null;
            ModPlayer? modPlayer = null;
            switch (messageType)
            {
                case PlayerPacketType.WeaponPlayer:
                    incomingDTO = DeserializeDTO<WeaponPlayerDTO>(json);
                    modPlayer = GetModPlayerFromPacket<WeaponPlayer>(fromWho);
                    break;

                case PlayerPacketType.ColorChipPlayer:
                    incomingDTO = DeserializeDTO<ColorChipPlayerDTO>(json);
                    modPlayer = GetModPlayerFromPacket<ColorChipPlayer>(fromWho);
                    break;
            }

            if (incomingDTO == null) return;

            if (IsThisTheServer())
            {
                // Server: forward this DTO to other players
                SendModPlayerPacket(
                    messageType: messageType,
                    fromWho: fromWho,
                    json: json,
                    logger: logger);
            }
            else
            {
                // Player: consume this DTO
                if (modPlayer != null)
                {
                    incomingDTO.ApplyToModPlayer(modPlayer);
                }
                else
                {
                    DebugHelper.PrintWarning($"Tried to consume {nameof(ModPlayer)} DTO packet, but the value of {nameof(modPlayer)} was null.");
                }
            }
        }

        private static T? DeserializeDTO<T>(string json)
            where T : BaseDTO
        {
            var dto = JsonConvert.DeserializeObject<T>(json);

            if (dto == null)
            {
                DebugHelper.PrintWarning($"Tried to deserialize DTO, but the result was {dto}");
            }
            else
            {
                DebugHelper.PrintInfo($"Received JSON:\n{json}");
            }

            return dto;
        }

        private static T GetModPlayerFromPacket<T>(int fromWho)
            where T : ModPlayer
        {
            Player p = GetPlayerFromPacket(fromWho);
            return p.GetModPlayer<T>();
        }
    }
}
