using System;

namespace Wisplantern.Systems
{
    class AuraTiles : ModSystem
    {
        public static bool moonflower = false;
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            moonflower = tileCounts[ModContent.TileType<Tiles.Moonflower>()] > 0;
        }
    }
}
