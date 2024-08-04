using Terraria.WorldBuilding;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.IO;

namespace Wisplantern.Systems.Worldgen
{
    class LushGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            tasks.Insert(genIndex + 1, new PassLegacy("Lush", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Livening Things Up";

                if (Wisplantern.generateLushCaves)
                {
                    LushPatches();
                }
            }));
        }

        public static void TryPlaceLushPlant(int i, int j)
        {

            if (Main.tileSolid[Main.tile[i, j + 1].TileType] &&
                Main.tile[i, j + 1].Slope == SlopeType.Solid &&
                !WorldGen.TileEmpty(i, j + 1) &&
                Main.tile[i, j + 1].TileType == ModContent.TileType<Tiles.LushGrass>() &&
                WorldGen.genRand.NextBool(400))
            {
                WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.Plantscalibur>(), forced: false);
            }

            if (Main.tileSolid[Main.tile[i, j - 1].TileType] &&
                Main.tile[i, j - 1].Slope == SlopeType.Solid &&
                !WorldGen.TileEmpty(i, j - 1) &&
                Main.tile[i, j - 1].TileType == ModContent.TileType<Tiles.LushGrass>() &&
                Main.tileSolid[Main.tile[i + 1, j - 1].TileType] &&
                Main.tile[i + 1, j - 1].Slope == SlopeType.Solid &&
                !WorldGen.TileEmpty(i + 1, j - 1) &&
                Main.tile[i + 1, j - 1].TileType == ModContent.TileType<Tiles.LushGrass>() &&
                WorldGen.TileEmpty(i, j) && 
                WorldGen.TileEmpty(i + 1, j) &&
                WorldGen.genRand.NextBool(8))
            {
                WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.LushLantern>());
            }


            if (Main.tileSolid[Main.tile[i, j + 1].TileType] &&
                Main.tile[i, j + 1].Slope == SlopeType.Solid &&
                !WorldGen.TileEmpty(i, j + 1) &&
                Main.tile[i, j + 1].TileType == ModContent.TileType<Tiles.LushGrass>() &&
                Main.tileSolid[Main.tile[i + 1, j + 1].TileType] &&
                Main.tile[i + 1, j + 1].Slope == SlopeType.Solid &&
                !WorldGen.TileEmpty(i + 1, j + 1) &&
                Main.tile[i + 1, j + 1].TileType == ModContent.TileType<Tiles.LushGrass>() &&
                WorldGen.genRand.NextBool(8))
            {
                WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.Moonflower>(), forced: false);
            }

            if (Main.tileSolid[Main.tile[i, j + 1].TileType] &&
                Main.tile[i, j + 1].Slope == SlopeType.Solid &&
                !WorldGen.TileEmpty(i, j + 1) &&
                Main.tile[i, j + 1].TileType == ModContent.TileType<Tiles.LushGrass>() &&
                WorldGen.genRand.NextBool(30))
            {
                WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.GhostRose_1>(), forced: false);
            }

            if (Main.tileSolid[Main.tile[i, j + 1].TileType] &&
                Main.tile[i, j + 1].Slope == SlopeType.Solid &&
                !WorldGen.TileEmpty(i, j + 1) &&
                Main.tile[i, j + 1].TileType == ModContent.TileType<Tiles.LushGrass>() &&
                WorldGen.genRand.NextBool(30))
            {
                WorldGen.PlaceTile(i, j, ModContent.TileType<Tiles.GhostRose_2>(), forced: false);
            }
        }

        public void LushPatches()
        {
            FastNoiseLite noise = new(WorldGen.genRand.Next(1, 5000));
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.01f);
            noise.SetFractalOctaves(5);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.5f);
            noise.SetFractalPingPongStrength(2f);

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
                TileID.PurpleMoss
            };

            for (int i = 10; i < Main.maxTilesX - 10; i++)
            {
                for (int j = (int)GenVars.rockLayer; j < Main.maxTilesY - 10; j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile != null)
                    {
                        if (noise.GetNoise(i / 12f, j / 12f) >= 0.75f)
                        {
                            if (convertableStoneTiles.Contains(tile.TileType))
                            {
                                tile.TileType = TileID.Dirt;
                            }

                            if (tile.TileType == TileID.Dirt && WispUtils.TileCanBeLush(i, j))
                            {
                                tile.TileType = (ushort)ModContent.TileType<Tiles.LushGrass>();
                            }

                            if (tile.LiquidType == LiquidID.Lava && j < Main.UnderworldLayer)
                            {
                                tile.LiquidType = LiquidID.Water;
                            }

                            if (tile.TileType == TileID.LavaDrip)
                            {
                                tile.TileType = TileID.WaterDrip;
                            }

                            if (!tile.HasTile && Main.rand.NextBool(8) && j < Main.UnderworldLayer)
                            {
                                WorldGen.PlaceLiquid(i, j, (byte)LiquidID.Water, 255);
                            }
                        }
                    }
                }
            }

            for (int i = 10; i < Main.maxTilesX - 10; i++)
            {
                for (int j = (int)GenVars.rockLayer; j < Main.maxTilesY - 10; j++)
                {
                    if (noise.GetNoise(i / 12f, j / 12f) >= 0.75f)
                    {
                        TryPlaceLushPlant(i, j);
                    }
                }
            }
        }
    }
}
