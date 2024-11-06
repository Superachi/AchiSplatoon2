using AchiSplatoon2.Content.CustomConditions;
using AchiSplatoon2.Content.Items.Accessories;
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
                shop.Add<SplatBomb>();
                shop.Add<BurstBomb>(Condition.DownedEarlygameBoss);
                shop.Add<AngleShooter>(Condition.DownedEarlygameBoss);
                shop.Add<Sprinkler>(BossConditions.DownedEvilBoss);
                shop.Add<InkMine>(BossConditions.DownedEvilBoss);
                shop.Add<Torpedo>(Condition.Hardmode);
            }

            if (shop.NpcType == NPCID.Mechanic)
            {
                shop.Add<ChargedBattery>(Condition.DownedMechBossAny);
            }

            if (shop.NpcType == NPCID.Painter)
            {
                shop.Add<PainterDualie>(Condition.DownedEarlygameBoss);
            }
        }
    }
}
