using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.IO;

namespace Wisplantern.Systems.Worldgen
{
    class DepthrockGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Jungle"));
            tasks.Insert(genIndex + 1, new PassLegacy("Depthrock", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Rubbing Stones";

                DepthrockPatches();
            }));
        }

        public void DepthrockPatches()
        {
            FastNoiseLite noise = new FastNoiseLite(WorldGen.genRand.Next(1, 5000));
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.01f);
            noise.SetFractalOctaves(5);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.5f);
            noise.SetFractalPingPongStrength(2f);

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile != null)
                    {
                        if (tile.TileType == TileID.Stone && noise.GetNoise(i, j) >= MathHelper.Lerp(0.95f, 0.45f, (float)j / Main.maxTilesY))
                        {
                            tile.TileType = (ushort)ModContent.TileType<Tiles.Depthrock>();
                        }
                    }
                }
            }
        }
    }
}
