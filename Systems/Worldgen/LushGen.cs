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
    class LushGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            tasks.Insert(genIndex + 1, new PassLegacy("Lush", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Livening Things Up";

                LushPatches();
            }));
        }

        public void LushPatches()
        {
            FastNoiseLite noise = new FastNoiseLite(WorldGen.genRand.Next(1, 5000));
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.01f);
            noise.SetFractalOctaves(5);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.5f);
            noise.SetFractalPingPongStrength(2f);

            for (int i = 2; i < Main.maxTilesX - 2; i++)
            {
                for (int j = (int)WorldGen.rockLayer; j < Main.maxTilesY - 2; j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile != null)
                    {
                        if (noise.GetNoise(i / 12f, j / 12f) >= 0.75f)
                        {
                            if (tile.TileType == TileID.Stone)
                            {
                                tile.TileType = TileID.Dirt;
                            }
                            if (tile.TileType == TileID.Dirt && WispUtils.TileCanBeLush(i, j))
                            {
                                tile.TileType = (ushort)ModContent.TileType<Tiles.LushGrass>();
                            }
                        }
                    }
                }
            }
        }
    }
}
