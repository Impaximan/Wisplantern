using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class DemoniteCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 20;
            Item.SetManipulativePower(0.17f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 46;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 320f;

        public override int DustType => DustID.ShadowbeamStaff;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
            {
                Projectile.NewProjectile(new EntitySource_OnHit(player, target, "Demonite cane hit"), 
                    target.Center, 
                    Main.rand.NextFloat((float)Math.PI * 2f).ToRotationVector2() * Main.rand.NextFloat(5f, 10f), 
                    ModContent.ProjectileType<DemoniteWisp>(), 
                    (int)(Item.damage * 1.5f), 
                    Item.knockBack,
                    player.whoAmI,
                    target.whoAmI);

                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, target.Center);
            }
        }
    }

    class DemoniteWisp : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Lerp(Color.White, Color.Blue, 1f - ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.ShadowFlame, 180);

                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, target.Center);
            }
        }


        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 14;
            Projectile.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(-8, 0);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return timeAlive >= timeUntilHoming;
        }

        const int timeUntilHoming = 30;
        int timeAlive = 0;
        float angleLerpAmount = 0f;

        public override void AI()
        {
            timeAlive++;


            Projectile.rotation = Projectile.velocity.ToRotation();

            if (timeAlive >= timeUntilHoming)
            {
                angleLerpAmount += 0.1f / 60f;

                if (timeAlive > timeUntilHoming * 2f)
                {
                    angleLerpAmount += 1f / 30f;
                }

                if (angleLerpAmount > 1f)
                {
                    angleLerpAmount = 1f;
                }

                float distance = 1000f;

                NPC target = null;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    float dist = npc.Distance(Projectile.Center);

                    if (npc.active && !npc.friendly && dist < distance)
                    {
                        target = npc;
                        distance = dist;
                    }
                }

                if (target != null)
                {
                    Projectile.alpha = 0;

                    Projectile.velocity = Projectile.velocity.ToRotation().AngleLerp(Projectile.DirectionTo(target.Center).ToRotation(), angleLerpAmount).ToRotationVector2() * (Projectile.velocity.Length() + 0.1f);
                }
                else
                {
                    Projectile.velocity *= 0.93f;
                    Projectile.alpha += 5;

                    if (Projectile.alpha > 255)
                    {
                        Projectile.active = false;
                    }
                }
            }
        }
    }
}
