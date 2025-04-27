using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Ammo
{
    public class PandemoniumBullet : ModItem
    {
        public const int musketBallsNeeded = 200;
        public override void AddRecipes()
        {
            CreateRecipe(musketBallsNeeded)
                .AddIngredient<PandemoniumBar>()
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
            Item.width = 24;
            Item.height = 24;
            Item.damage = 12;
            Item.knockBack = 6.5f;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<PandemoniumBulletProjectile>();
            Item.shootSpeed = 11f;
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.maxStack = 9999;
        }
    }

    public class PandemoniumBulletProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(-4, 14);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!target.boss && target.type != NPCID.DungeonGuardian)
            {
                if (Main.rand.Next(target.lifeMax) <= Math.Clamp(Projectile.damage / 2, 0, target.lifeMax / 2))
                {
                    modifiers.SetInstantKill();
                    SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/Collapse");
                    style.PitchVariance = 0.7f;
                    style.Volume *= 0.5f;
                    SoundEngine.PlaySound(style, target.Center);
                    CombatText.NewText(target.getRect(), Color.LimeGreen, "INSTAKILL", true);
                }
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, new Vector3(125, 255, 255) / 1200f);
        }
    }
}