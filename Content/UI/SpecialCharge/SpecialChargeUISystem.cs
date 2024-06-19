using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace AchiSplatoon2.Content.UI.SpecialCharge
{
    [Autoload(Side = ModSide.Client)]
    internal class SpecialChargeUISystem : ModSystem
    {
        private UserInterface SpecialChargeBarUserInterface;

        internal SpecialChargeBar SpecialChargeBar;

        //public static LocalizedText ExampleResourceText { get; private set; }

        public override void Load()
        {
            SpecialChargeBar = new();
            SpecialChargeBarUserInterface = new();
            SpecialChargeBarUserInterface.SetState(SpecialChargeBar);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            SpecialChargeBarUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "ExampleMod: Example Resource Bar",
                    delegate {
                        SpecialChargeBarUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
