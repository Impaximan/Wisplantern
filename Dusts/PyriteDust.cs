namespace Wisplantern.Dusts
{
    class PyriteDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.frame = new Rectangle(0, Main.rand.Next(2) * 6, 6, 6);
            dust.scale = Main.rand.NextFloat(1f, 1.3f);
            base.OnSpawn(dust);
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            if (Collision.SolidCollision(dust.position, 2, 2))
            {
                dust.velocity = Vector2.Zero;
            }
            else
            {
                if (!dust.noGravity)
                {
                    dust.velocity.Y += 0.2f;
                }
            }
            dust.scale -= 1f / 60f;
            if (dust.scale <= 0f)
            {
                dust.active = false;
            }

            return false;
        }
    }
}
