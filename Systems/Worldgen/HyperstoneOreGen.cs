using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.Utilities;
using Terraria.DataStructures;
using System;

namespace Wisplantern.Systems.Worldgen
{
    class HyperstoneOreGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            tasks.Insert(genIndex + 1, new PassLegacy("Hyperstone", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Getting Some Electrolytes";

                Hyperstone();
            }));
        }

        public void Hyperstone()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 2); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(8, 10), WorldGen.genRand.Next(1, 3), ModContent.TileType<Tiles.Hyperstone>(), false, 0f, 0f, false, true);
            }
        }
    }
}
