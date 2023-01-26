using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.Utilities;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.UI;
using System.Linq;

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
