using AchiSplatoon2.Content.CustomConditions;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class ShopGlobalNPC : BaseGlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Demolitionist)
            {
                shop.Add<SplatBomb>(BossConditions.DownedEvilBoss);
                shop.Add<BurstBomb>();
                shop.Add<AngleShooter>(Condition.DownedEarlygameBoss);
                shop.Add<Sprinkler>(Condition.DownedEarlygameBoss);
            }

            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add<OrderShot>();
                shop.Add<OrderBrush>();
                shop.Add<OrderStringer>();
                shop.Add<OrderCharger>();
            }
        }
    }
}
