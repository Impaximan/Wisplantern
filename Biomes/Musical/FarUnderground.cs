using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Wisplantern.Biomes
{
    class FarUnderground : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

        public override int Music => MusicLoader.GetMusicSlot("Wisplantern/Sounds/Music/FarUnderground");

        public override bool IsBiomeActive(Player player)
        {
            int checkedY = (int)MathHelper.Lerp((int)Main.rockLayer, Main.UnderworldLayer, 0.5f) + 85;
            //Main.NewText("Player Y: " + (int)player.position.Y + "\nChecked Y: " + checkedY + "\nChecked Y x16: " + checkedY * 16);
            return player.ZoneRockLayerHeight && player.position.Y > checkedY * 16;
        }
    }
}
