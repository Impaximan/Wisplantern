namespace Wisplantern.Items.Equipable.Accessories
{
    public class DraconicDacron : ModItem
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
            Item.SetScholarlyDescription("Found in chests deep underground within the lava layer");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<DraconicDacronPlayer>().equipped = true;
        }
    }

    class DraconicDacronPlayer : ModPlayer
    {
        public bool equipped = false;

        public override void ResetEffects()
        {
            equipped = false;
        }
    }

    class DraconicDacronItem : GlobalItem
    {
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.GetModPlayer<DraconicDacronPlayer>().equipped)
            {
                if (type == ProjectileID.WoodenArrowFriendly && Main.rand.NextBool(6))
                {
                    type = ProjectileID.FireArrow;
                    damage += 2;
                }
            }
        }
    }

    class DraconicDacronProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[projectile.owner].GetModPlayer<DraconicDacronPlayer>().equipped)
            {
                if (projectile.type == ProjectileID.FireArrow)
                {
                    target.AddBuff(BuffID.OnFire, 180);
                }
                if (projectile.type == ProjectileID.FrostburnArrow)
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }
        }
    }
}
