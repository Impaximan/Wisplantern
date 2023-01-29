using Terraria.ModLoader.Config;
using System.ComponentModel;
using Terraria;

namespace Wisplantern
{
    [BackgroundColor(56 / 5, 69 / 5, 119 / 5, (int)(255f * 0.75f))]
    class ServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return Main.countsAsHostForGameplay[whoAmI];
        }

        [Header("Optional Worldgen")]

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        [Label("Abandoned Hellevator")]
        [Tooltip("Whether or not the 'Abandoned Hellevator' structure will generate (requires you to generate a new world in order to affect anything)")]
        public bool generateHellevator;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        [Label("Underground Lakes")]
        [Tooltip("Whether or not the 'Underground Lakes' will generate (requires you to generate a new world in order to affect anything)")]
        public bool generateLakes;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        [Label("Frost Fortress")]
        [Tooltip("Whether or not the 'Frost Fortress' microdungeon will generate (requires you to generate a new world in order to affect anything)")]
        public bool generateFrostFortress;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        [Label("Canyon")]
        [Tooltip("Whether or not the 'Canyon' will generate (requires you to generate a new world in order to affect anything)")]
        public bool generateCanyon;

        [DefaultValue(true)]
        [BackgroundColor(123, 225, 255)]
        [Label("Chastised Church")]
        [Tooltip("Whether or not the 'Chastised Church' microdungeon will generate (requires you to generate a new world in order to affect anything)")]
        public bool generateChastisedChurch;

        public override void OnChanged()
        {
            Wisplantern.generateCanyons = generateCanyon;
            Wisplantern.generateFrostFortresses = generateFrostFortress;
            Wisplantern.generateLakes = generateLakes;
            Wisplantern.generateHellevators = generateHellevator;
            Wisplantern.generateChastisedChurch = generateChastisedChurch;
        }
    }
}
