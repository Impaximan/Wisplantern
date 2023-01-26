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
    class IgneousCaveGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            tasks.Insert(genIndex + 1, new PassLegacy("IgneousCave", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Volcanic Activity";

                IgneousPatches();
            }));
        }

        public void IgneousPatches()
        {
            FastNoiseLite noise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.01f);
            noise.SetFractalOctaves(5);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.5f);
            noise.SetFractalPingPongStrength(2f);

            List<int> cancelingTiles = new List<int>()
            {
                TileID.IceBlock,
                TileID.JungleGrass,
                TileID.MushroomGrass,
                TileID.SnowBlock,
                ModContent.TileType<Tiles.LushGrass>(),
                TileID.HardenedSand,
                TileID.Sandstone
            };

            List<int> convertableStoneTiles = new List<int>()
            {
                TileID.Stone,
                TileID.ArgonMoss,
                TileID.BlueMoss,
                TileID.BrownMoss,
                TileID.GreenMoss,
                TileID.KryptonMoss,
                TileID.LavaMoss,
                TileID.RedMoss,
                TileID.XenonMoss,
                TileID.PurpleMoss,
                ModContent.TileType<Tiles.Depthrock>()
            };

            int extraCheckAmount = 10;
            for (int i = extraCheckAmount + 1; i < Main.maxTilesX - extraCheckAmount - 1; i++)
            {
                for (int j = (int)WorldGen.rockLayer; j < Main.maxTilesY - extraCheckAmount - 1; j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile != null)
                    {
                        if (noise.GetNoise(i / 12f, j / 12f) >= 0.75f)
                        {
                            bool canConvert = true;
                            for (int i2 = -extraCheckAmount; i2 <= extraCheckAmount; i2++)
                            {
                                for (int j2 = -extraCheckAmount; j2 <= extraCheckAmount; j2++)
                                {
                                    Tile tile2 = Main.tile[i + i2, j + j2];
                                    if (tile2 != null)
                                    {
                                        if (cancelingTiles.Contains(tile2.TileType))
                                        {
                                            canConvert = false;
                                        }
                                    }
                                }
                            }

                            if (canConvert)
                            {
                                if (convertableStoneTiles.Contains(tile.TileType))
                                {
                                    tile.TileType = (ushort)ModContent.TileType<Tiles.IgneousStone>();
                                }

                                if (tile.TileType == TileID.Dirt)
                                {
                                    tile.TileType = TileID.ClayBlock;
                                }

                                if (tile.LiquidType == LiquidID.Water)
                                {
                                    tile.LiquidType = LiquidID.Lava;
                                }

                                if (!tile.HasTile && Main.rand.NextBool(5))
                                {
                                    WorldGen.PlaceLiquid(i, j, LiquidID.Lava, 255);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
