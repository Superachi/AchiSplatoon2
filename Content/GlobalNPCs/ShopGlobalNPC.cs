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
                shop.Add<StarterSplatBomb>();
                shop.Add<SplatBomb>(Condition.DownedEarlygameBoss);
                shop.Add<BurstBomb>(Condition.DownedEarlygameBoss);
                shop.Add<AngleShooter>(BossConditions.DownedEvilBoss);
                shop.Add<Sprinkler>(BossConditions.DownedEvilBoss);
                shop.Add<InkMine>(Condition.DownedSkeletron);
                shop.Add<Torpedo>(Condition.Hardmode);
            }

            if (shop.NpcType == NPCID.Mechanic)
            {
                shop.Add<ChargedBattery>(Condition.DownedMechBossAny);
            }

            if (shop.NpcType == NPCID.Painter)
            {
                shop.Add(ItemID.BlackInk);
                shop.Add<PainterDualie>(Condition.DownedEarlygameBoss);
            }
        }
    }
}
