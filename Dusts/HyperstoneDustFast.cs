namespace Wisplantern.Dusts
{
    class HyperstoneDustFast : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, Main.rand.Next(2) * 6, 6, 6);
            base.OnSpawn(dust);
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White * (1f - (dust.alpha / 255f));
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 1f / 10f;
            if (dust.scale <= 0f)
            {
                dust.active = false;
            }
            if (!dust.noLight) Lighting.AddLight(dust.position, 187 / 1200f * dust.scale, 206 / 1200f * dust.scale, 238 / 1200f * dust.scale);
            return false;
        }
    }
}
