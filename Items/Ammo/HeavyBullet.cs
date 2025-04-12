using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace Wisplantern.Items.Ammo
{
    public class HeavyBullet_Iron : ModItem
    {
        public const int musketBallsNeeded = 70;
        public override void AddRecipes()
        {
            CreateRecipe(musketBallsNeeded)
                .AddIngredient(ItemID.IronBar)
                .AddIngredient(ItemID.MusketBall, musketBallsNeeded)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.damage = 11;
            Item.knockBack = 6.5f;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<HeavyBullet>();
            Item.shootSpeed = 3.5f;
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.maxStack = 9999;
        }
    }

    public class HeavyBullet_Lead : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe(HeavyBullet_Iron.musketBallsNeeded)
                .AddIngredient(ItemID.LeadBar)
                .AddIngredient(ItemID.MusketBall, HeavyBullet_Iron.musketBallsNeeded)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.damage = 11;
            Item.knockBack = 6.5f;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<HeavyBullet>();
            Item.shootSpeed = 3.5f;
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.maxStack = 9999;
        }
    }

    public class HeavyBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource)
            {
                ammoSource.Entity.velocity += Projectile.velocity.ToRotation().ToRotationVector2() * -1.25f;

                if (ammoSource.Entity.velocity.Length() < 20)
                {
                    Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15f)) * 1.25f;
                }
            }
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.hostile = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Lerp(Color.White, Color.Magenta, 1f - ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(-4, 0);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundStyle style = new("Wisplantern/Sounds/Effects/BulletWhizz");
            style.PitchVariance = 0.5f;
            style.Pitch = -0.75f;
            style.Volume *= 0.25f;
            style.MaxInstances = 1;
            style.SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest;
            SoundEngine.PlaySound(style, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center);
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, new Vector3(252, 186, 54) * 0.001f);
        }


    }
}
