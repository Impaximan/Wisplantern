using Terraria.ModLoader.IO;

namespace Wisplantern.Systems.Events
{
    class CalmNight : ModSystem
    {
        public static bool calmNight = false;
        int forcedCalmNightState = 0;

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("calmNight", calmNight);
            tag.Add("wasDay", wasDay);
            tag.Add("forcedCalmNightState", forcedCalmNightState);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            calmNight = tag.GetBool("calmNight");
            wasDay = tag.GetBool("wasDay");
            forcedCalmNightState = tag.GetInt("forcedCalmNightState");
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
                if (!Main.bloodMoon)
                {
                    if (Main.rand.NextBool(10) && Wisplantern.calmNights)
                    {
                        calmNight = true;
                    }

                    if (forcedCalmNightState == 0 || (forcedCalmNightState == 1 && Main.hardMode))
                    {
                        forcedCalmNightState++;
                        calmNight = true;
                    }

                    if (calmNight)
                    {
                        Main.NewText("The moon shimmers soothingly in the night sky...", Color.LightGreen);
                    }
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
