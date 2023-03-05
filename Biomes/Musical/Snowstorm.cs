using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Wisplantern.Biomes
{
    class Snowstorm : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override int Music => MusicLoader.GetMusicSlot("Wisplantern/Sounds/Music/SnowstormV2");

        public override bool IsBiomeActive(Player player)
        {
            //Main.NewText("Player Y: " + (int)player.position.Y + "\nChecked Y: " + checkedY + "\nChecked Y x16: " + checkedY * 16);
            return player.ZoneRain && player.ZoneSnow;
        }
    }
}
