using System;
using MonoMod.Cil;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using ReLogic.Utilities;

namespace Wisplantern
{
    internal class Detours
    {
        public static void Load()
        {
            On.Terraria.Main.DoUpdate += DoUpdate;
            On.Terraria.Main.DrawInterface += DrawOvertopGraphics;
        }

        public static void Unload()
        {
            On.Terraria.Main.DoUpdate -= DoUpdate;
            On.Terraria.Main.DrawInterface -= DrawOvertopGraphics;
        }

        public static void DoUpdate(On.Terraria.Main.orig_DoUpdate orig, Main self, ref GameTime gameTime)
        {
            if (Wisplantern.freezeFrames > 0)
            {
                Wisplantern.freezeFrames--;
            }
            else
            {
                Wisplantern.freezeFrameLight = false;
                orig(self, ref gameTime);
            }
        }

        static float freezeFrameLightAlpha = 0f;
        private static void DrawOvertopGraphics(On.Terraria.Main.orig_DrawInterface orig, Main self, GameTime gameTime)
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
    }
}
