using Terraria.ModLoader;
using System;
using Terraria.ModLoader.IO;
using Terraria;
using Microsoft.Xna.Framework;

namespace Wisplantern.Systems.Events
{
    class CalmNight : ModSystem
    {
        public static bool calmNight = false;

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("calmNight", calmNight);
            tag.Add("wasDay", wasDay);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            calmNight = tag.GetBool("calmNight");
            wasDay = tag.GetBool("wasDay");
        }

        bool wasDay = false;
        public override void PostUpdateEverything()
        {
            if (Main.dayTime)
            {
                wasDay = true;
                calmNight = false;
            }

            if (Main.bloodMoon)
            {
                calmNight = false;
            }

            if (!Main.dayTime && wasDay)
            {
                wasDay = false;
                if (Main.rand.NextBool(5) && !Main.bloodMoon)
                {
                    calmNight = true;
                }
            }
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (calmNight)
            {
                tileColor = Color.Lerp(tileColor, Color.Blue, 0.04f);
                backgroundColor = Color.Lerp(backgroundColor, Color.Blue, 0.08f);
            }
        }
    }
}
