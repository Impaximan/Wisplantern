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

        [Header("$Mods.Wisplantern.UIHeader")]

        [DefaultValue(0.78f)]
        [BackgroundColor(123, 225, 255)]
        [Increment(0.01f)]
        public float charismaX = 0f;

        [DefaultValue(0.06f)]
        [BackgroundColor(123, 225, 255)]
        [Increment(0.01f)]
        public float charismaY = 0f;

        [DefaultValue(CharismaBehavior.ShowOnlyWithManipulative)]
        [BackgroundColor(123, 225, 255)]
        public CharismaBehavior charismaBehavior;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool pushCharismaDown = true;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        public bool noBattleArtTooltip = true;

        public override void OnChanged()
        {
            Wisplantern.playDeepUnderground = playDeepUnderground;
            Wisplantern.playSnowstorm = playSnowstorm;
            Wisplantern.playClairDeLune = playClairDeLune;

            Wisplantern.classTags = classTags;

            Wisplantern.charismaX = charismaX;
            Wisplantern.charismaY = charismaY;
            Wisplantern.pushCharismaDown = pushCharismaDown;
            Wisplantern.charismaBehavior = charismaBehavior;
        }
    }

    public enum CharismaBehavior : byte
    {
        ShowOnlyWithManipulative = 0,
        ShowAlways = 1,
        ShowNever = 2,
    }
}
