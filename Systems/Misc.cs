using System;
using Terraria.ModLoader.IO;

namespace Wisplantern.Systems
{
    class Misc : ModSystem
    {
        public static bool moonflower = false;
        public static bool HuntressSaved = false;

        public override void PostWorldGen()
        {
            HuntressSaved = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("HuntressSaved", HuntressSaved);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            HuntressSaved = tag.GetBool("HuntressSaved");
        }


        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            moonflower = tileCounts[ModContent.TileType<Tiles.Moonflower>()] > 0;
        }
    }
}
