using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System;

namespace Wisplantern.Items.Weapons.Magic.Staffs
{
    class Plantscalibur : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Unleashes a lingering, blinding light that confuses victims" +
                "\nDoes more damage the closer the target is"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<PlantscaliburSpotlight>();
            Item.width = 68;
            Item.height = 44;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.mana = 10;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shootSpeed = 1f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }

    class PlantscaliburSpotlight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Plantscalibur beam");
        }

        Vector2 properScale;

        public override void OnSpawn(IEntitySource source)
        {
            properScale = new Vector2(0.01f, 0.2f);
            Projectile.alpha = 255;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 7;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= MathHelper.Lerp(1f - target.Distance(Main.player[Projectile.owner].Center) / 1000f, 1f, 0.35f);
            modifiers.Knockback *= MathHelper.Lerp(1f - target.Distance(Main.player[Projectile.owner].Center) / 500f, 1f, 0.1f);
            if (target.Distance(Main.player[Projectile.owner].Center) > 500f)
            {
                modifiers.Knockback.Base = 0f;
            }
        }

        int timeLeftWhenDisappear = 30;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity.Normalize();
            properScale = Vector2.Lerp(properScale, new Vector2(1f, 1f), 0.1f);
            if (Projectile.timeLeft < timeLeftWhenDisappear)
            {
                Projectile.alpha += 255 / timeLeftWhenDisappear;
                properScale.X *= 0.8f;
            }
            else if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
            }
            Projectile.Center = Main.player[Projectile.owner].Center;

            for (int i = 0; i < (int)(1000f * properScale.Y); i += 10)
            {
                float divideAmount = 2f;
                float distanceMult = 1f - i / 1000f;
                float alphaMult = 1f - (Projectile.alpha / 255f);
                Lighting.AddLight(Projectile.Center + Projectile.velocity * i, 0.7f / divideAmount * alphaMult * distanceMult, 0.9f / divideAmount * alphaMult * distanceMult, 1f / divideAmount * alphaMult * distanceMult);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(12) && target.Distance(Main.player[Projectile.owner].Center) <= 650f)
            {
                target.AddBuff(BuffID.Confused, 60 * 2);
                target.netUpdate = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            float maxDistance = 1000 * properScale.Y;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + unit * maxDistance, 75 * properScale.X, ref point);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White) * 0.75f, Projectile.rotation - MathF.PI * 0.5f, new Vector2(100, 0), properScale, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }
    }
}
