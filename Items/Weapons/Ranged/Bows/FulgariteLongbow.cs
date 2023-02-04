using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Ranged.Bows
{
    class FulgariteLongbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Fires supercharged arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;
            Item.width = 20;
            Item.height = 46;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[p].GetGlobalProjectile<FulgariteLongbowArrow>().supercharged = true;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Placeable.Blocks.Fulgarite>(25)
                .AddIngredient(ItemID.Emerald, 2)
                .AddIngredient(ItemID.WhiteString)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class FulgariteLongbowArrow : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool supercharged = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            supercharged = false;
        }

        public override void PostAI(Projectile projectile)
        {
            if (supercharged)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.GreenTorch);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity = Vector2.Zero;
                dust.scale = 1.5f;
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && supercharged)
            {
                Vector2 velocity = projectile.velocity;
                velocity.Normalize();
                Projectile.NewProjectile(projectile.GetSource_OnHit(target, "FulgariteLongbow"), projectile.Center, velocity * 10f, ModContent.ProjectileType<FulgariteLongbowBolt>(), projectile.damage / 2, projectile.knockBack / 4, projectile.owner);
            }
        }
    }

    class FulgariteLongbowBolt : ModProjectile
    {
        public override string Texture => "Wisplantern/Items/Weapons/Ranged/Bows/FulgariteLongbow";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fulgarite Lightning");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 15;
            Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GreenTorch);
            dust.noGravity = true;
            dust.noLight = true;
            dust.velocity = Vector2.Zero;
            dust.scale = 1.5f;
        }
    }
}
