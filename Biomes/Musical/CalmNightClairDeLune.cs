namespace Wisplantern.Biomes
{
    class CalmNightClairDeLune : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override int Music => MusicLoader.GetMusicSlot("Wisplantern/Sounds/Music/ClaudeDebussyClairDeLune");

        public override bool IsBiomeActive(Player player)
        {
            return !Main.dayTime && player.ZoneOverworldHeight && Systems.Events.CalmNight.calmNight && Wisplantern.playClairDeLune;
        }
    }
}
