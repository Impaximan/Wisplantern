namespace Wisplantern.Items.Equipable.Accessories
{
    class Flint : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.SetScholarlyDescription("Extracted from silt, the flint is a handy stone when it comes to fire");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<FlintPlayer>().equipped = true;
        }
    }

    class FlintPlayer : ModPlayer
    {
        public bool equipped = false;

        public override void ResetEffects()
        {
            equipped = false;
        }
    }

    class FlintNPC : GlobalNPC
    {
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (player.GetModPlayer<FlintPlayer>().equipped && item.DamageType is DamageClasses.ManipulativeDamageClass)
            {
                npc.AddBuff(BuffID.OnFire, 60 * 3);
            }
        }
    }
}
