using AchiSplatoon2.Content.Players;
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
using Microsoft.Xna.Framework;
using static AchiSplatoon2.Helpers.NetHelper;
using log4net.Repository.Hierarchy;
using AchiSplatoon2.Netcode.DataTransferObjects;
using Terraria.ID;

namespace AchiSplatoon2.Netcode
{
    enum PlayerPacketType : byte
    {
        SpecialReady,
        UpdateInkColor,
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
            _logger.Info($"Received '{msgType}' packet.");

            switch (msgType)
            {
                case (int)PlayerPacketType.SyncModPlayer:
                    ReceiveSyncPlayer(_reader, _whoAmI, _logger);
                    break;
                default:
                    _logger.WarnFormat("Unknown Message type: {0}", msgType);
                    break;
            }
        }

        public static void SendSyncPlayer(int fromWho, InkWeaponPlayerDTO dto, ILog logger)
        {
            if (IsSinglePlayer()) { return; }

            // Prepare data
            Player player = Main.LocalPlayer;
            ModPacket packet = GetNewPacket();

            // Write
            // 'Headers'
            WritePacketHandlerType(packet, (int)PacketHandlerType.Player);
            WritePacketType(packet, (int)PlayerPacketType.SyncModPlayer);
            WritePacketFromWhoID(packet, fromWho);

            // 'Payload'
            packet.Write(dto.SpecialReady);
            packet.WriteRGB(dto.InkColor);

            // Send
            SendPacket(packet, toClient: -1, ignoreClient: fromWho);
        }

        public static void ReceiveSyncPlayer(BinaryReader reader, int fromWho, ILog logger)
        {
            // Read data
            // 'Headers'
            fromWho = reader.ReadInt32();

            // 'Payload'
            bool specialReady = reader.ReadBoolean();
            Color colorFromChips = reader.ReadRGB();
            var dto = new InkWeaponPlayerDTO(specialReady, colorFromChips);

            // Respond
            if (IsThisTheServer())
            {
                // Forward
                SendSyncPlayer(fromWho, dto, logger);
            }
            else
            {
                var modPlayer = GetModPlayerFromPacket(fromWho);
                modPlayer.SpecialReady = specialReady;
                modPlayer.ColorFromChips = colorFromChips;
            }
        }
    }
}
