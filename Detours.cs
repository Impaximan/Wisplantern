using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace Wisplantern
{
    internal class Detours
    {
        public static void Load()
        {
            Terraria.On_Main.DoUpdate += DoUpdate;
            Terraria.On_Main.DrawInterface += DrawOvertopGraphics;
			Terraria.On_Rain.MakeRain += MakeRain;
		}

        public static void Unload()
        {
            Terraria.On_Main.DoUpdate -= DoUpdate;
            Terraria.On_Main.DrawInterface -= DrawOvertopGraphics;
			Terraria.On_Rain.MakeRain -= MakeRain;
		}

        public static void DoUpdate(Terraria.On_Main.orig_DoUpdate orig, Main self, ref GameTime gameTime)
        {
            if (Wisplantern.freezeFrames > 0)
            {
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    Wisplantern.freezeFrames = 0;
                    orig(self, ref gameTime);
                }
                Wisplantern.freezeFrames--;
            }
            else
            {
                Wisplantern.freezeFrameLight = false;
                orig(self, ref gameTime);
            }
        }

        static float freezeFrameLightAlpha = 0f;
        private static void DrawOvertopGraphics(Terraria.On_Main.orig_DrawInterface orig, Main self, GameTime gameTime)
        {
            orig(self, gameTime);

            Main.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            if (Wisplantern.freezeFrames > 0 && Wisplantern.freezeFrameLight)
            {
                freezeFrameLightAlpha = 0.15f;
            }
            else
            {
                freezeFrameLightAlpha *= 0.8f;
            }

            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Wisplantern/WhitePixel"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * freezeFrameLightAlpha);

            Main.spriteBatch.End();
        }

		private static void MakeRain(Terraria.On_Rain.orig_MakeRain orig)
		{
			if (!Systems.Events.Snowstorm.snowing)
			{
				orig();
			}
			else
			{
				if (Main.LocalPlayer != null)
				{
					if (Main.LocalPlayer.ZoneJungle || Main.LocalPlayer.ZoneDesert)
					{
						orig();
						return;
					}
				}

				if (Main.netMode == 2 || Main.gamePaused || (double)Main.screenPosition.Y > Main.worldSurface * 16.0 || Main.gameMenu)
				{
					return;
				}
				float num = (float)Main.screenWidth / 1920f;
				num *= 25f;
				num *= 0.25f + 1f * Main.cloudAlpha;
				if (Filters.Scene["Sandstorm"].IsActive())
				{
					return;
				}
				Vector2 vector = default(Vector2);
				for (int i = 0; (float)i < num; i++)
				{
					int num2 = 1200;
					if (Main.player[Main.myPlayer].velocity.Y < 0f)
					{
						num2 += (int)(Math.Abs(Main.player[Main.myPlayer].velocity.Y) * 60f);
					}
					vector.X = Main.rand.Next((int)Main.screenPosition.X - num2, (int)Main.screenPosition.X + Main.screenWidth + num2);
					vector.Y = Main.screenPosition.Y - (float)Main.rand.Next(20, 100);
					vector.X -= Main.windSpeedCurrent * 15f * 40f * 2f;
					vector.X += Main.player[Main.myPlayer].velocity.X * 40f;
					if (vector.X < 0f)
					{
						vector.X = 0f;
					}
					if (vector.X > (float)((Main.maxTilesX - 1) * 16))
					{
						vector.X = (Main.maxTilesX - 1) * 16;
					}
					int num3 = (int)vector.X / 16;
					int num4 = (int)vector.Y / 16;
					if (num3 < 0)
					{
						num3 = 0;
					}
					if (num3 > Main.maxTilesX - 1)
					{
						num3 = Main.maxTilesX - 1;
					}
					if (num4 < 0)
					{
						num4 = 0;
					}
					if (num4 > Main.maxTilesY - 1)
					{
						num4 = Main.maxTilesY - 1;
					}
					if (Main.gameMenu || (!WorldGen.SolidTile(num3, num4) && Main.tile[num3, num4].WallType <= 0))
					{
						if (Main.rand.NextFloat() <= 0.75f)
						{
							Vector2 rainFallVelocity = Rain.GetRainFallVelocity();
							Terraria.Dust dust = Dust.NewDustPerfect(vector, 76);
							//dust.noGravity = false;
							dust.velocity = rainFallVelocity;
							dust.velocity.Y /= 2;
						}
					}
				}
			}
		}
	}
}
