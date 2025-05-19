using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace Wisplantern.Items.Ammo
{
    public class PrimitiveDart : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 28;
            Item.damage = 8;
            Item.knockBack = 1f;
            Item.ammo = AmmoID.Dart;
            Item.shoot = ModContent.ProjectileType<PrimitiveDartProjectile>();
            Item.shootSpeed = 5f;
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 0, 50);
        }
    }

    public class PrimitiveDartProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(4, -10);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.05f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
