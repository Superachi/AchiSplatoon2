using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Consumables.Potions;
using AchiSplatoon2.Content.Players;
using System.Collections.Generic;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Commands
{
    internal class HelpCommand : ModCommand
    {
        // CommandType.World means that command can be used in Chat in SP and MP, but executes on the Server in MP
        public override CommandType Type
            => CommandType.World;

        public override string Command
            => "woomyTip";

        public override string Usage
            => "/woomyTip";

        public override string Description
            => "Provides a tip on how to progress further";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            var strings = GenerateProgressionTip();
            ChatHelper.SendChatToThisClient($"Tip: {Main.rand.NextFromCollection(strings)}", Color.Orange);
        }

        private static List<string> GenerateProgressionTip()
        {
            var player = Main.LocalPlayer;
            var list = new List<string>();

            // PRE-HARDMODE
            if (!Condition.Hardmode.IsMet())
            {
                list.Add("When in Swim Form, you take no fall damage!");
                list.Add("You recover ink faster while in Swim Form and while standing still.");

                if (!Condition.DownedEyeOfCthulhu.IsMet() && !Condition.DownedKingSlime.IsMet())
                {
                    list.Add("You can craft more Order weapons after you've collected some Silver or Tungsten.");
                    return list;
                }

                list.Add("Now that you've defeated a boss, all enemies can drop Basic Sheldon Licenses... Sometimes.");
                list.Add("There are some weapons out there that don't require a license, but you'll have to gather specific ingredients to make them.");

                if (!Condition.PlayerCarriesItem(ModContent.ItemType<InkCapacityPotion>()).IsMet()
                    && !Condition.PlayerCarriesItem(ModContent.ItemType<InkRegenerationPotion>()).IsMet())
                {
                    list.Add("If you can find some Black Ink, you can brew potions that increase your ink capacity and recovery speed.");
                }

                if (!Condition.NpcIsPresent(NPCID.Demolitionist).IsMet())
                {
                    list.Add("The Demolitionist can sell you different kinds of sub weapons! If you can get him to move in...");
                }

                if (Condition.DownedEowOrBoc.IsMet())
                {
                    list.Add("Meteorite and Hellstone both give you access to powerful weapons to help defeat upcoming bosses.");
                    list.Add("Now that you have access to Meteorite, you can craft more Color Chips to use with your Palette.");
                    return list;
                }
            }

            // HARDMODE
            else
            {
                list.Add("All enemies can drop Silver Sheldon Licenses, but mimics are guaranteed to drop them!");
                list.Add("Large mimics are dangerous, but they drop accessories that can alter the effects of certain main weapons.");
                list.Add("Certain flying invasion bosses drop parts of an extremely powerful Color Chip Palette.");

                if (!player.GetModPlayer<InkTankPlayer>().HasUsedAllCrystals())
                {
                    list.Add("You can use Crystal Shards, combined with other items, to increase your ink capacity permanently.");
                }

                if (!Condition.DownedMechBossAny.IsMet())
                {
                    list.Add("Try digging up some of the new ores and see what weapons you can craft! You might find some familiar designs with a stronger damage output.");
                    return list;
                }

                if (Condition.DownedPlantera.IsMet())
                {
                    list.Add("Duke Fishron, The Empress of Light and final bosses from the Pumpkin Moon and Frost Moon all drop special weaponry that can't simply be crafted.");
                }

                if (Condition.DownedGolem.IsMet())
                {
                    if (!Condition.DownedMartians.IsMet())
                    {
                        list.Add("The Martians are in possession of a powerful Brella that deals shocking damage.");
                        return list;
                    }
                }
            }

            return list;
        }
    }
}
