using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

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

    class FlintItem : GlobalItem
    {
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.GetModPlayer<FlintPlayer>().equipped)
            {
                if (type == ProjectileID.WoodenArrowFriendly && Main.rand.NextBool(6))
                {
                    type = ProjectileID.FireArrow;
                    damage += 2;
                }
            }
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

    class FlintProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[projectile.owner].GetModPlayer<FlintPlayer>().equipped)
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
