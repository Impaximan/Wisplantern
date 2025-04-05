using Terraria.WorldBuilding;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.IO;

namespace Wisplantern.Systems.Worldgen
{
    class OreGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            tasks.Insert(genIndex + 1, new PassLegacy("Wisplantern Shinies", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Shinier shinies";

                Hyperstone();
                Pandemonium();
            }));
        }

        public void Hyperstone()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 2); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(8, 10), WorldGen.genRand.Next(1, 3), ModContent.TileType<Tiles.Hyperstone>(), false, 0f, 0f, false, true);
            }
        }

        public void Pandemonium()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 10); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(12, 16), WorldGen.genRand.Next(2, 4), ModContent.TileType<Tiles.PandemoniumOre>(), false, 0f, 0f, false, true);
            }
        }
    }
}
