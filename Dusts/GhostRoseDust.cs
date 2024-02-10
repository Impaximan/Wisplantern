namespace Wisplantern.Dusts
{
    class GhostRoseDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, Main.rand.Next(2) * 6, 6, 6);
            dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
            rotateDirection = Main.rand.NextBool() ? -1 : 1;
            rotateSpeed = Main.rand.NextFloat(0.5f, 3f);
            base.OnSpawn(dust);
        }

        int rotateDirection = 0;
        float rotateSpeed = 0f;
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            if (Collision.SolidCollision(dust.position, 6, 6))
            {
                dust.scale -= 1f / 60f;
                if (dust.scale <= 0f)
                {
                    dust.active = false;
                }
                dust.velocity = Vector2.Zero;
            }
            dust.velocity = dust.velocity.RotatedBy(MathHelper.ToRadians(rotateSpeed) * rotateDirection);
            if (Main.rand.NextBool(90))
            {
                rotateDirection *= -1;
            }
            if (!dust.noLight) Lighting.AddLight(dust.position, 255 / 2000f * dust.scale, 200 / 2000f * dust.scale, 255 / 2000f * dust.scale);
            return false;
        }
    }
}
