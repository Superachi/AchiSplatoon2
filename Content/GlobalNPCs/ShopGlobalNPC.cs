using AchiSplatoon2.Content.CustomConditions;
using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
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
                shop.Add<StarterSplatBomb>();
                shop.Add<BurstBomb>(Condition.DownedEarlygameBoss);
                shop.Add<PointSensor>(Condition.DownedEarlygameBoss);
                shop.Add<SplatBomb>(BossConditions.DownedEvilBoss);
                shop.Add<AngleShooter>(BossConditions.DownedEvilBoss);
                shop.Add<Sprinkler>(Condition.DownedSkeletron);
                shop.Add<InkMine>(Condition.DownedSkeletron);
                shop.Add<Torpedo>(Condition.Hardmode);
            }

            if (shop.NpcType == NPCID.Mechanic)
            {
                shop.Add<ChargedBattery>(Condition.Hardmode);
            }

            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add<InkDroplet>();
            }

            if (shop.NpcType == NPCID.Painter)
            {
                shop.Add<PainterDualie>(Condition.DownedEarlygameBoss);
            }
        }
    }
}
