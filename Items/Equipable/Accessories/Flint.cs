﻿using Microsoft.Xna.Framework;
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
            Tooltip.SetDefault("Perfectly timed strikes with zweihanders set enemies ablaze" +
                "\nChance to set wooden arrows ablaze" +
                "\nGuarantees that flaming arrows will ignite enemies");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 3, 0, 0);
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

    class FlintProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
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