using Terraria.WorldBuilding;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.IO;
using System;

namespace Wisplantern.Systems.Worldgen
{
    class IgneousCaveGen : ModSystem
    {
        FastNoiseLite noise;

        List<int> cancelingTiles = new()
            {
                TileID.IceBlock,
                TileID.JungleGrass,
                TileID.MushroomGrass,
                TileID.SnowBlock,
                ModContent.TileType<Tiles.LushGrass>(),
                TileID.HardenedSand,
                TileID.Sandstone
            };

        List<int> convertableStoneTiles = new()
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

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
            tasks.Insert(genIndex + 1, new PassLegacy("Igneous City", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Burying A City";

                if (Wisplantern.generateVolcanicCaves)
                {
                    IgneousCity();
                }
            }));
            
            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            tasks.Insert(genIndex + 1, new PassLegacy("Igneous Cave", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Volcanic Activity";

                if (Wisplantern.generateVolcanicCaves)
                {
                    IgneousPatches();
                }
            }));
        }

        public bool CanConvertTile(int i, int j, int extraCheckAmount)
        {
            for (int i2 = -extraCheckAmount; i2 <= extraCheckAmount; i2++)
            {
                for (int j2 = -extraCheckAmount; j2 <= extraCheckAmount; j2++)
                {
                    Tile tile2 = Main.tile[i + i2, j + j2];
                    if (tile2 != null)
                    {
                        if (cancelingTiles.Contains(tile2.TileType))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool ShouldConvertTile(int i, int j, int extraCheckAmount)
        {
            if (noise.GetNoise((i + 8000) / 12f, (j + 8000) / 12f) >= 0.75f)
            {
                return CanConvertTile(i, j, extraCheckAmount);
            }
            return false;
        }

        public int GetRandomFromInt(int min, int max, int number, int seed)
        {
            Random rand = new(seed + number);

            return rand.Next(min, max);
        }

        public void IgneousCity()
        {
            #region Stuff I dont need to see
            noise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.01f);
            noise.SetFractalOctaves(5);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.5f);
            noise.SetFractalPingPongStrength(2f);

            FastNoiseLite lackNoise = new(WorldGen.genRand.Next(5000, 10000));
            lackNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            lackNoise.SetFrequency(0.01f);
            lackNoise.SetFractalOctaves(5);
            lackNoise.SetFractalLacunarity(2f);
            lackNoise.SetFractalGain(0.5f);
            lackNoise.SetFractalPingPongStrength(2f);

            FastNoiseLite lackNoise2 = new(WorldGen.genRand.Next(5000, 10000));
            lackNoise2.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            lackNoise2.SetFrequency(0.01f);
            lackNoise2.SetFractalOctaves(5);
            lackNoise2.SetFractalLacunarity(2f);
            lackNoise2.SetFractalGain(0.5f);
            lackNoise2.SetFractalPingPongStrength(2f);

            cancelingTiles = new()
            {
                TileID.IceBlock,
                TileID.JungleGrass,
                TileID.MushroomGrass,
                TileID.SnowBlock,
                ModContent.TileType<Tiles.LushGrass>(),
                TileID.HardenedSand,
                TileID.Sandstone
            };

            convertableStoneTiles = new()
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

            #endregion

            int hallwayWidth = 15;
            int hallwayHeight = 20;
            int hallwaySpacingY = 75;
            int hallwaySpacingX = 140;

            int tilingType = TileID.IronBrick;
            int brickType = ModContent.TileType<Tiles.BlackBrick>();
            int pipingType = TileID.CopperBrick;
            int smoothType = ModContent.TileType<Tiles.SmoothDepthrock>();
            int oreType = TileID.Iron;

            int smoothWallType = ModContent.WallType<Tiles.SmoothDepthrockWall>();
            int pipingWallType = WallID.CopperBrick;
            int fenceWallType = WallID.WroughtIronFence;
            int brickWallType = ModContent.WallType<Tiles.BlackBrickWall>();

            int seedThing = WorldGen.genRand.Next(500);

            for (int i = 10 + 1; i < Main.maxTilesX - 10 - 1; i++)
            {
                for (int j = (int)GenVars.rockLayer; j < Main.maxTilesY - 200; j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile != null)
                    {
                        if (ShouldConvertTile(i, j, 10) && lackNoise.GetNoise(i, j) <= 0.35f)
                        {
                            //int jOffset = GetRandomOffsetFromInt(15, j - (j % hallwaySpacingY), seedThing + 1);

                            int hI = i % hallwaySpacingX;
                            int hJ = j % hallwaySpacingY;

                            int hallwayType = GetRandomFromInt(0, 2, j - j % hallwaySpacingY, seedThing);

                            if (hI < hallwayWidth && hJ < hallwayHeight)
                            {
                                WorldGen.KillTile(i, j);

                                if ((hI <= 3 || hI >= hallwayWidth - 4) && (hJ < 3 || hJ > hallwayHeight - 3))
                                {
                                    WorldGen.PlaceTile(i, j, smoothType, true, true);
                                }

                                if (lackNoise2.GetNoise(i * 4f, j * 4f) <= 0.25f)
                                {
                                    WorldGen.PlaceWall(i, j, smoothWallType, true);

                                    if (hI == hallwayWidth / 2)
                                    {
                                        WorldGen.PlaceTile(i, j, TileID.Chain, true, true);
                                    }
                                }
                            }
                            else if (hI < hallwayWidth)
                            {
                                WorldGen.KillTile(i, j);

                                if (hI <= 2 || hI >= hallwayWidth - 3)
                                {
                                    WorldGen.PlaceTile(i, j, smoothType, true, true);
                                }

                                if (lackNoise2.GetNoise(i * 4f, j * 4f) <= 0.25f)
                                {
                                    WorldGen.PlaceWall(i, j, smoothWallType, true);

                                    if (hI == hallwayWidth / 2)
                                    {
                                        WorldGen.PlaceTile(i, j, TileID.Chain, true, true);
                                    }
                                }
                            }
                            else if (hJ < hallwayHeight)
                            {
                                WorldGen.KillTile(i, j);
                                if (hJ == hallwayHeight - 3)
                                {
                                    WorldGen.PlaceTile(i, j, tilingType, true, true);
                                }
                                else if (hJ < 3 || hJ > hallwayHeight - 3)
                                {
                                    WorldGen.PlaceTile(i, j, brickType, true, true);
                                }

                                if (hallwayType == 0)
                                {
                                    if (hJ > hallwayHeight * 0.6f + WorldGen.genRand.Next(3))
                                    {
                                        WorldGen.PlaceWall(i, j, fenceWallType, true);
                                    }
                                    if (hJ == hallwayHeight - 4)
                                    {
                                        if (i % 10 == 0)
                                        {
                                            WorldGen.PlaceTile(i, j, TileID.Lampposts, true, true);
                                        }
                                    }
                                }
                                else if (lackNoise2.GetNoise(i * 4f, j * 4f) <= 0.25f)
                                {
                                    WorldGen.PlaceWall(i, j, brickWallType, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void IgneousPatches()
        {
            cancelingTiles = new List<int>()
            {
                TileID.IceBlock,
                TileID.JungleGrass,
                TileID.MushroomGrass,
                TileID.SnowBlock,
                ModContent.TileType<Tiles.LushGrass>(),
                TileID.HardenedSand,
                TileID.Sandstone
            };

            convertableStoneTiles = new List<int>()
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

            for (int i = 10 + 1; i < Main.maxTilesX - 10 - 1; i++)
            {
                for (int j = (int)GenVars.rockLayer; j < Main.maxTilesY - 200 - 1; j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile != null)
                    {
                        if (ShouldConvertTile(i, j, 10))
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

                            if (tile.TileType == TileID.WaterDrip)
                            {
                                tile.TileType = TileID.LavaDrip;
                            }

                            if (!tile.HasTile && Main.rand.NextBool(5))
                            {
                                WorldGen.PlaceLiquid(i, j, (byte)LiquidID.Lava, 255);
                            }
                        }
                    }
                }
            }
        }
    }
}
