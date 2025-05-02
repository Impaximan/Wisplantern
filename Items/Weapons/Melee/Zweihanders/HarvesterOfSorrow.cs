using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Wisplantern.Buffs;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    public class HarvesterOfSorrow : Zweihander
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void ZweihanderDefaults()
        {
            Item.knockBack = 5f;
            Item.width = 72;
            Item.height = 64;
            Item.damage = 52;
            Item.shootSpeed = 11f;
            Item.rare = ItemRarityID.Orange;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 0, 54, 0);
        }

        public override bool HasSwungDust => true;

        public override int SwungDustType => DustID.Lava;

        public override float SwungDustSize => 0.5f;

        public override void OnHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 60 * 5);

            if (perfectCharge)
            {
                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, target.Center);

                for (int i = 0; i < Main.rand.Next(4, 7); i++)
                {
                    Projectile projectile = Main.projectile[Projectile.NewProjectile(new EntitySource_OnHit(player, target),
                        target.Center,
                        new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-10f, -5f)),
                        ModContent.ProjectileType<MagmaBlob>(),
                        (int)player.GetDamage(DamageClass.Melee).ApplyTo(30f),
                        0.5f,
                        player.whoAmI)];

                    projectile.ai[0] = target.whoAmI;
                    projectile.netUpdate = true;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1f - (Item.alpha / 255f));
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 20)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.FieryGreatsword);
        }
    }

    public class MagmaBlob : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = Projectile.oldPos.Length - 1; i >= 0; i--)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Lerp(Color.White, Color.Magenta, 1f - ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.ai[1] = 0;
            Projectile.hostile = false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.velocity.Y <= 0)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 60 * 8);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);

            for (int i = 0; i < Main.rand.Next(6, 12); i++)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch)];
                d.scale *= Main.rand.NextFloat(2f, 3f);
                d.velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), -Main.rand.NextFloat(1f, 10f));
            }
        }

        Vector2 ogPosition = Vector2.Zero;
        public override void AI()
        {
            if (ogPosition == Vector2.Zero)
            {
                ogPosition = Projectile.Center;
            }

            Projectile.velocity.Y += 0.05f;
            Projectile.velocity *= 0.99f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.tileCollide = Projectile.Center.Y >= ogPosition.Y;

            NPC target = Main.npc[(int)Projectile.ai[0]];
            bool hasTarget = false;
            if (!target.active)
            {
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
            }
            else
            {
                hasTarget = true;
            }
            if (Projectile.ai[0] == 0f)
            {
                int t = Projectile.FindTargetWithLineOfSight(400f);
                if (t != -1)
                {
                    target = Main.npc[t];
                    hasTarget = true;
                }
                
            }

            if (hasTarget)
            {
                Projectile.velocity.X += Math.Sign(target.Center.X - Projectile.Center.X) * 0.025f;
            }
        }
    }
}