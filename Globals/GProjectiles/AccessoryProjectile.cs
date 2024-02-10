using Wisplantern.Items.Equipable.Accessories;

namespace Wisplantern.Globals.GProjectiles
{
    public class AccessoryProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[projectile.owner].AccessoryActive<DraconicDacron>())
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
