using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using System;

namespace Wisplantern.Items.Weapons.Magic.Misc
{
    class AccursedShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\nA mysterious shard, carrying a curse" +
                "\nCan be flung telekinetically at enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<AccursedShardProjectile>();
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.mana = 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item8;
            Item.shootSpeed = 3f;
            Item.value = Item.sellPrice(0, 0, 25, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                int p = Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))) * Main.rand.NextFloat(2f) - velocity * 2, type, damage, knockback, player.whoAmI);
                Main.projectile[p].ai[0] = velocity.X;
                Main.projectile[p].ai[1] = velocity.Y;
            }
            return false;
        }
    }

    class AccursedShardProjectile : ModProjectile
    {
        public override string Texture => "Wisplantern/Items/Weapons/Magic/Misc/AccursedShard";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Accursed Shard");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (tileCollideTime > 25)
            {
                return true;
            }

            if (oldVelocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = oldVelocity.X * -1;
            }

            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                Projectile.velocity.Y = oldVelocity.Y * -1;
            }

            return false;
        }

        int tileCollideTime = 0;
        float rotationAmount = 0f;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = MathHelper.ToRadians(Main.rand.Next(360));
            rotationAmount = Main.rand.NextFloat(-7f, 7f);
        }

        public override void AI()
        {
            Projectile.velocity += new Vector2(Projectile.ai[0], Projectile.ai[1]) * 0.25f;
            Projectile.rotation += rotationAmount;
            tileCollideTime++;
        }
    }
}
