using Terraria.Audio;
using Terraria.GameContent;

namespace Wisplantern.Items.Equipable.Accessories
{
    public class FrontlinersSpearhead : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 7, 50, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);
            player.GetDamage(DamageClass.Summon) += 0.08f;
        }
    }

    public class FrontlinersSpearheadSpear : ModProjectile
    {
        public static int totalTime = 30;

        public override void SetDefaults()
        {
            totalTime = 30;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = totalTime;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile owner = Main.projectile[(int)Projectile.ai[0]];

            if (!owner.active)
            {
                Projectile.active = false;
                return;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            float maxDistance = 60;
            Projectile.Center = owner.Center + Projectile.velocity *
                (Projectile.timeLeft > totalTime / 2 ? (totalTime - Projectile.timeLeft) * (maxDistance / totalTime * 2f) : maxDistance - (totalTime / 2 - Projectile.timeLeft) * (maxDistance / totalTime * 2f));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(9f, 14f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }

    public class FrontlinersSpearheadJavelin : ModProjectile
    {
        public override string Texture => "Wisplantern/Items/Equipable/Accessories/FrontlinersSpearheadSpear";

        int numHits = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            return base.OnTileCollide(oldVelocity);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                for (float i = -1f; i < 1f; i += 1f / 10f)
                {
                    Vector2 position = Projectile.Center + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * (i * (float)Math.Sqrt(Projectile.width * Projectile.height) - 35f) + Main.rand.NextVector2Circular(3f, 3f);
                    Dust d = Dust.NewDustPerfect(position, DustID.DynastyWood, Projectile.velocity / 10f, Scale: (i + 2f) * 0.5f);
                    d.noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        int gravityCounter = 0;
        public override void AI()
        {
            gravityCounter++;
            if (gravityCounter > 15) Projectile.velocity.Y += 0.4f;
            Projectile.velocity *= 0.98f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
