using Terraria.ModLoader.Config;
using System.ComponentModel;
using Terraria;
using Terraria.Localization;

namespace Wisplantern
{
    [BackgroundColor(56 / 5, 69 / 5, 119 / 5, (int)(255f * 0.75f))]
    class ServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
        {
            return Main.countsAsHostForGameplay[whoAmI];
        }

        [Header("$Mods.Wisplantern.WorldgenHeader")]

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool generateMassiveMountain;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool generatePits;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool generateLushCaves;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool generateVolcanicCaves;


        [Header("$Mods.Wisplantern.MusicHeader")]

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool playDeepUnderground = true;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool playSnowstorm = true;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool playClairDeLune = true;

        [Header("$Mods.Wisplantern.EventHeader")]

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool calmNights = true;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool snowstorms = true;

        public override void OnChanged()
        {
            Wisplantern.generateMassiveMountain = generateMassiveMountain;
            Wisplantern.generatePits = generatePits;
            Wisplantern.generateLushCaves = generateLushCaves;
            Wisplantern.generateVolcanicCaves = generateVolcanicCaves;

            Wisplantern.playDeepUnderground = playDeepUnderground;
            Wisplantern.playSnowstorm = playSnowstorm;
            Wisplantern.playClairDeLune = playClairDeLune;

            Wisplantern.calmNights = calmNights;
            Wisplantern.snowstorms = snowstorms;
        }
    }
}
