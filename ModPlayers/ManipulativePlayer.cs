using Terraria.ModLoader.IO;

namespace Wisplantern.ModPlayers
{
    class ManipulativePlayer : ModPlayer
    {
        public int charisma = 0;
        public int defMaxCharisma = 3;
        public int extraMaxCharisma = 0;
        public float manipulativePower = 1f;

        public int MaxCharisma => defMaxCharisma + extraMaxCharisma;

        public override void SaveData(TagCompound tag)
        {
            tag.Add("maxCharisma", defMaxCharisma);
            tag.Add("charisma", charisma);
        }

        public override void LoadData(TagCompound tag)
        {
            defMaxCharisma = tag.GetInt("maxCharisma");
            charisma = tag.GetInt("charisma");
        }

        public override void ResetEffects()
        {
            extraMaxCharisma = 0;
            manipulativePower = 1f;
        }

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

        public override void PostUpdate()
        {
            if (charisma > MaxCharisma)
            {
                charisma = MaxCharisma;
            }
        }
    }
}