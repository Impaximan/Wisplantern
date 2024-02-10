namespace Wisplantern.ModPlayers
{
    class ManipulativePlayer : ModPlayer
    {
        public int smokeBombTime = 0;
        public override void PreUpdateBuffs()
        {
            if (smokeBombTime > 0)
            {
                smokeBombTime--;
                Player.immune = true;
                Player.immuneAlpha = 255;
                Player.immuneTime = 15;
                Player.moveSpeed *= 2f;
                Player.jumpSpeedBoost += 3f;
                int d = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke);
                Main.dust[d].velocity += new Vector2(Player.velocity.X, Player.velocity.Y - Main.rand.Next(5));
                if (smokeBombTime <= 0)
                {
                    Player.SmokeBombEffect();
                }
            }
        }
    }
}