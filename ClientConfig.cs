using Terraria.ModLoader.Config;
using System.ComponentModel;
using Terraria;
using Terraria.Localization;

namespace Wisplantern
{
    [BackgroundColor(56 / 5, 69 / 5, 119 / 5, (int)(255f * 0.75f))]
    class ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

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

        [Header("$Mods.Wisplantern.TooltipHeader")]

        [DefaultValue(false)]
        [BackgroundColor(123, 225, 255)]
        public bool classTags = true;

        public override void OnChanged()
        {
            Wisplantern.playDeepUnderground = playDeepUnderground;
            Wisplantern.playSnowstorm = playSnowstorm;
            Wisplantern.playClairDeLune = playClairDeLune;

            Wisplantern.classTags = classTags;
        }
    }
}
