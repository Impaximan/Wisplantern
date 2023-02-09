using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Biomes.CaveHouse;
using IL.Terraria.GameContent.UI.States;

namespace Wisplantern.Systems.Worldgen
{
    class MiscGen : ModSystem
    {
        //NOTES:
        //Pillars and other surface tile stuff go after "Full Desert" (with Fulgarite)
        //Special caves go after "Wavy Caves"

        //TODO:
        //Make mountain work with various different biomes again

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            GenPass jungleGrass = tasks.Find(x => x.Name.Equals("Mud Caves To Grass"));

            tasks.Remove(jungleGrass);

            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Wavy Caves"));

            tasks.Insert(genIndex + 1, new PassLegacy("Special Cave Shapes", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Massive Caves";
                LargeCaves();


                progress.Message = "Sine Caves";
                SineCaves();

                progress.Message = "Fissures from Hell";
                FissuresFromHell();

            }));

            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

            tasks.Insert(genIndex + 1, new PassLegacy("Mountain Shinies", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Mountain Shines";
                foreach (Tuple<Point, int> ore in oresToPlace)
                {
                    WorldGen.PlaceTile(ore.Item1.X, ore.Item1.Y, ore.Item2, true, false);
                }
            }));

            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Full Desert"));

            tasks.Insert(genIndex + 1, jungleGrass);

            tasks.Insert(genIndex + 1, new PassLegacy("Mountain", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Massive Mountain";
                Mountain();
            }));

            tasks.Insert(genIndex + 1, new PassLegacy("Misc Surface Structures", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Striking the Earth";
                Fulgarite();
            }));

            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Buried Chests"));

            tasks.Insert(genIndex + 1, new PassLegacy("Mountain Chests", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Mountain Chests";
                for (int i = 0; i < mountainRect.Width / 30; i++)
                {
                    Point currentPosition = new Point(Main.rand.Next(mountainRect.X + mountainRect.Width / 4, (int)(mountainRect.X + mountainRect.Width * 0.75f)), Main.rand.Next(mountainRect.Y, mountainRect.Y + mountainRect.Height));
                    if (!(Main.tile[currentPosition.X, currentPosition.Y].HasTile && Main.tileSolid[Main.tile[currentPosition.X, currentPosition.Y].TileType]))
                    {
                        while (!WorldUtils.Find(currentPosition, Searches.Chain(new Searches.Down(1), new GenCondition[]
                            {
        new Conditions.IsSolid()
                            }), out _))
                        {
                            currentPosition.Y++;
                        }

                        WorldGen.KillTile(currentPosition.X, currentPosition.Y - 1);
                        WorldGen.KillTile(currentPosition.X + 1, currentPosition.Y - 1);
                        WorldGen.KillTile(currentPosition.X, currentPosition.Y);
                        WorldGen.KillTile(currentPosition.X + 1, currentPosition.Y);
                        WorldGen.PlaceTile(currentPosition.X, currentPosition.Y + 1, TileID.WoodBlock);
                        WorldGen.SlopeTile(currentPosition.X, currentPosition.Y + 1, 0);
                        WorldGen.PlaceTile(currentPosition.X + 1, currentPosition.Y + 1, TileID.WoodBlock);
                        WorldGen.SlopeTile(currentPosition.X + 1, currentPosition.Y + 1, 0);

                        WorldGen.PlaceChest(currentPosition.X, currentPosition.Y);

                        #region Chest Loot
                        if (Chest.FindChest(currentPosition.X, currentPosition.Y - 1) > 0)
                        {
                            Chest chest = Main.chest[Chest.FindChest(currentPosition.X, currentPosition.Y - 1)];
                            if (chest != null)
                            {
                                List<int> mainItems = new List<int>
                                {
                                    ItemID.Spear,
                                    ItemID.Blowpipe,
                                    ItemID.WoodenBoomerang,
                                    ItemID.Aglet,
                                    ItemID.ClimbingClaws,
                                    ItemID.Umbrella,
                                    ItemID.CordageGuide,
                                    ItemID.WandofSparking,
                                    ItemID.Radar,
                                    ItemID.PortableStool
                                };

                                List<int> ammosAndThrowables = new List<int>
                                {
                                    ItemID.WoodenArrow,
                                    ItemID.FlamingArrow,
                                    ItemID.Shuriken,
                                    ItemID.ThrowingKnife
                                };

                                List<int> bars = new List<int>
                                {
                                WorldGen.SavedOreTiers.Copper == TileID.Copper ? ItemID.CopperBar : ItemID.TinBar,
                                WorldGen.SavedOreTiers.Iron == TileID.Iron ? ItemID.IronBar : ItemID.LeadBar,
                                WorldGen.SavedOreTiers.Silver == TileID.Silver ? ItemID.SilverBar : ItemID.TungstenBar,
                                WorldGen.SavedOreTiers.Gold == TileID.Gold ? ItemID.GoldBar : ItemID.PlatinumBar,
                                };

                                List<int> commonPotions = new List<int>
                                {
                                    ItemID.IronskinPotion,
                                    ItemID.ShinePotion,
                                    ItemID.NightOwlPotion,
                                    ItemID.SwiftnessPotion,
                                    ItemID.FeatherfallPotion,
                                    ItemID.GravitationPotion
                                };

                                List<int> lightItems = new List<int>
                                {
                                    ItemID.Torch,
                                    ItemID.Glowstick
                                };

                                int item = 0;
                                chest.item[item].SetDefaults(WorldGen.genRand.Next(mainItems));
                                item++;
                                if (WorldGen.genRand.NextFloat() <= 0.1f)
                                {
                                    chest.item[item].SetDefaults(ItemID.SlimeCrown);
                                    item++;
                                }
                                if (WorldGen.genRand.NextFloat() <= 0.33f)
                                {
                                    chest.item[item].SetDefaults(ItemID.Dynamite);
                                    item++;
                                }
                                if (WorldGen.genRand.NextFloat() <= 0.5f)
                                {
                                    chest.item[item].SetDefaults(WorldGen.genRand.Next(bars));
                                    chest.item[item].stack = WorldGen.genRand.Next(5, 10);
                                    item++;
                                }
                                if (WorldGen.genRand.NextFloat() <= 0.5f)
                                {
                                    chest.item[item].SetDefaults(WorldGen.genRand.Next(ammosAndThrowables));
                                    chest.item[item].stack = WorldGen.genRand.Next(25, 51);
                                    item++;
                                }
                                if (WorldGen.genRand.NextFloat() <= 0.5f)
                                {
                                    chest.item[item].SetDefaults(ItemID.LesserHealingPotion);
                                    chest.item[item].stack = WorldGen.genRand.Next(3, 6);
                                    item++;
                                }
                                if (WorldGen.genRand.NextFloat() <= 0.666f)
                                {
                                    chest.item[item].SetDefaults(WorldGen.genRand.Next(commonPotions));
                                    chest.item[item].stack = WorldGen.genRand.Next(2, 5);
                                    item++;
                                }
                                if (WorldGen.genRand.NextFloat() <= 0.666f)
                                {
                                    chest.item[item].SetDefaults(ItemID.RecallPotion);
                                    chest.item[item].stack = WorldGen.genRand.Next(3, 6);
                                    item++;
                                }
                                if (WorldGen.genRand.NextFloat() <= 0.5f)
                                {
                                    chest.item[item].SetDefaults(WorldGen.genRand.Next(lightItems));
                                    chest.item[item].stack = WorldGen.genRand.Next(20, 40);
                                    item++;
                                }
                                if (WorldGen.genRand.NextFloat() <= 0.5f)
                                {
                                    chest.item[item].SetDefaults(ItemID.SilverCoin);
                                    chest.item[item].stack = WorldGen.genRand.Next(15, 45);
                                    item++;
                                }
                            }
                        }
                        #endregion
                    }
                }
            }));

            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));

            tasks.Insert(genIndex + 1, new PassLegacy("Mountain Cleanup", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Cleaning Up Mountain";

                List<int> snowWalls = new()
                {
                    WallID.Dirt,
                    WallID.GrassUnsafe,
                };

                List<int> iceWalls = new()
                {
                    WallID.Stone,
                    WallID.Cave5Unsafe,
                };

                List<int> mudWalls = new()
                {
                    WallID.Dirt,
                    WallID.GrassUnsafe,
                };

                for (int i = -1000; i < 1000; i++)
                {
                    for (int j = -1000; j < 1000; j++)
                    {
                        int rI = mountainPosition.ToPoint().X + i;
                        int rJ = mountainPosition.ToPoint().Y + j;
                        if (rI > 0 && rI < Main.maxTilesX && rJ > 0 && rJ < Main.maxTilesY)
                        {
                            if (snowWalls.Contains(Main.tile[rI, rJ].WallType))
                            {
                                if (CheckForTile(rI, rJ, TileID.SnowBlock)) Main.tile[rI, rJ].WallType = WallID.SnowWallUnsafe;
                            }
                            if (iceWalls.Contains(Main.tile[rI, rJ].WallType))
                            {
                                if (CheckForTile(rI, rJ, TileID.IceBlock)) Main.tile[rI, rJ].WallType = WallID.IceUnsafe;
                            }
                            if (mudWalls.Contains(Main.tile[rI, rJ].WallType))
                            {
                                if (CheckForTile(rI, rJ, TileID.Mud)) Main.tile[rI, rJ].WallType = WallID.MudUnsafe;
                            }
                        }
                    }
                }
            }));
        }

        bool CheckForTile(int i, int j, int type, int size = 15)
        {
            for (int x = -size; x < size; x++)
            {
                for (int y = -size; y < size; y++)
                {
                    if (i > size && i < Main.maxTilesX - size && j > size && j < Main.maxTilesY - size)
                    {
                        if (Main.tile[i + x, j + y].TileType == type)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #region Surface Obstacles

        void SurfaceObstacles()
        {
            int amount = 20;

            for (int a = 0; a < amount; a++)
            {
                int side = WorldGen.genRand.NextBool() ? 1 : -1;
                Vector2 currentPosition = new Vector2(Main.maxTilesX / 2 + side * WorldGen.genRand.Next(0, Main.maxTilesX / 2 - 300), 100);
                while (!WorldUtils.Find(currentPosition.ToPoint(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                    {
        new Conditions.IsSolid()
                    }), out _))
                {
                    currentPosition.Y++;
                }


                Point position = currentPosition.ToPoint();

                FastNoiseLite noise1 = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
                noise1.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
                noise1.SetFrequency(0.01f);
                noise1.SetFractalOctaves(5);
                noise1.SetFractalLacunarity(2f);
                noise1.SetFractalGain(0.5f);
                noise1.SetFractalPingPongStrength(2f);

                switch (WorldGen.genRand.Next(1))
                {
                    case 0: //Stone pillars
                        int height = 20;
                        for (int i = -WorldGen.genRand.Next(3, 6); i < WorldGen.genRand.Next(3, 6); i++)
                        {
                            for (int j = -height + (int)(Math.Abs(i) * (2 + noise1.GetNoise((position.X + i) * 5f, 0f))); j < 4; j++)
                            {
                                int rI = position.X + i;
                                int rJ = position.Y + j;
                                rI += (int)(7f * noise1.GetNoise((position.Y + j) * 3f, 0f));
                                WorldGen.PlaceTile(rI, rJ, TileID.Stone);
                            }
                        }
                        break;
                }

            }
        }

        #endregion

        #region Mountain

        Vector2 mountainPosition;
        List<Tuple<Point, int>> oresToPlace = new List<Tuple<Point, int>>();
        Rectangle mountainRect = new Rectangle();
        void Mountain()
        {
            #region Noise
            FastNoiseLite mountainNoise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            mountainNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            mountainNoise.SetFrequency(0.01f);
            mountainNoise.SetFractalOctaves(5);
            mountainNoise.SetFractalLacunarity(2f);
            mountainNoise.SetFractalGain(0.5f);
            mountainNoise.SetFractalPingPongStrength(2f);

            FastNoiseLite dirtNoise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            dirtNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            dirtNoise.SetFrequency(0.01f);
            dirtNoise.SetFractalOctaves(5);
            dirtNoise.SetFractalLacunarity(2f);
            dirtNoise.SetFractalGain(0.5f);
            dirtNoise.SetFractalPingPongStrength(2f);

            FastNoiseLite wallNoise1 = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            wallNoise1.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            wallNoise1.SetFrequency(0.01f);
            wallNoise1.SetFractalOctaves(5);
            wallNoise1.SetFractalLacunarity(2f);
            wallNoise1.SetFractalGain(0.5f);
            wallNoise1.SetFractalPingPongStrength(2f);

            FastNoiseLite wallNoise2 = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            wallNoise2.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            wallNoise2.SetFrequency(0.01f);
            wallNoise2.SetFractalOctaves(5);
            wallNoise2.SetFractalLacunarity(2f);
            wallNoise2.SetFractalGain(0.5f);
            wallNoise2.SetFractalPingPongStrength(2f);

            FastNoiseLite wallNoise3 = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));

            FastNoiseLite caveNoise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            caveNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            caveNoise.SetFrequency(0.01f);
            caveNoise.SetFractalOctaves(5);
            caveNoise.SetFractalLacunarity(2f);
            caveNoise.SetFractalGain(0.5f);
            caveNoise.SetFractalPingPongStrength(2f);

            FastNoiseLite liquidNoise1 = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            liquidNoise1.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            liquidNoise1.SetFrequency(0.01f);
            liquidNoise1.SetFractalOctaves(5);
            liquidNoise1.SetFractalLacunarity(2f);
            liquidNoise1.SetFractalGain(0.5f);
            liquidNoise1.SetFractalPingPongStrength(2f);

            FastNoiseLite topNoise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            topNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            topNoise.SetFrequency(0.01f);
            topNoise.SetFractalOctaves(5);
            topNoise.SetFractalLacunarity(2f);
            topNoise.SetFractalGain(0.5f);
            topNoise.SetFractalPingPongStrength(2f);

            FastNoiseLite oreNoise1 = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            oreNoise1.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            oreNoise1.SetFrequency(0.01f);
            oreNoise1.SetFractalOctaves(5);
            oreNoise1.SetFractalLacunarity(2f);
            oreNoise1.SetFractalGain(0.5f);
            oreNoise1.SetFractalPingPongStrength(2f);

            FastNoiseLite oreNoise2 = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            oreNoise2.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            oreNoise2.SetFrequency(0.01f);
            oreNoise2.SetFractalOctaves(5);
            oreNoise2.SetFractalLacunarity(2f);
            oreNoise2.SetFractalGain(0.5f);
            oreNoise2.SetFractalPingPongStrength(2f);

            FastNoiseLite oreNoise3 = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            oreNoise3.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            oreNoise3.SetFrequency(0.01f);
            oreNoise3.SetFractalOctaves(5);
            oreNoise3.SetFractalLacunarity(2f);
            oreNoise3.SetFractalGain(0.5f);
            oreNoise3.SetFractalPingPongStrength(2f);

            FastNoiseLite snowNoise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
            snowNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            snowNoise.SetFrequency(0.01f);
            snowNoise.SetFractalOctaves(5);
            snowNoise.SetFractalLacunarity(2f);
            snowNoise.SetFractalGain(0.5f);
            snowNoise.SetFractalPingPongStrength(2f);
            #endregion

            oresToPlace.Clear();

            int side = WorldGen.genRand.NextBool() ? 1 : -1;
            Vector2 currentPosition = new Vector2(Main.maxTilesX / 2 + side * WorldGen.genRand.Next(0, Main.maxTilesX / 2 - 300), 100);
            while (!(Main.tileSolid[Main.tile[(int)currentPosition.X, (int)currentPosition.Y].TileType] || currentPosition.Y >= Main.maxTilesY * 0.75f))
            {
                currentPosition.Y++;
            }

            while (!WorldUtils.Find(currentPosition.ToPoint(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                currentPosition.Y++;
            }

            mountainPosition = currentPosition;
            Point position = currentPosition.ToPoint();
            position.Y += 20;

            int mountainHeight = position.Y / 2 + 50;
            int mountainWidth = (int)(mountainHeight * WorldGen.genRand.NextFloat(1.3f, 2f));

            mountainRect = new Rectangle(position.X - mountainWidth / 2, position.Y - mountainHeight, mountainWidth, mountainHeight);

            int soilType = TileID.Dirt;
            int stoneType = TileID.Stone;
            int snowType = TileID.SnowBlock;
            int iceType = TileID.IceBlock;
            int liquidType = LiquidID.Water;
            if (Main.rand.NextBool(4)) liquidType = LiquidID.Lava;

            int soilWall1 = WallID.GrassUnsafe;
            int soilWall2 = WallID.DirtUnsafe;
            int rockWall1 = Main.rand.Next(new List<int>()
            {
                WallID.CaveUnsafe,
                WallID.Cave2Unsafe,
                WallID.Cave3Unsafe,
                WallID.Cave4Unsafe,
                WallID.Cave5Unsafe,
                WallID.Cave6Unsafe,
                WallID.Cave7Unsafe,
                WallID.Cave8Unsafe,
            });
            int rockWall2 = WallID.Stone;
            int snowWall = WallID.SnowWallUnsafe;
            int iceWall = WallID.IceUnsafe;

            if (CheckForTile(position.X, position.Y, TileID.Sand, 25) || CheckForTile(position.X, position.Y, TileID.HardenedSand, 25))
            {
                soilType = TileID.HardenedSand;
                stoneType = TileID.Sandstone;
                soilWall1 = WallID.Sandstone;
                soilWall2 = WallID.HardenedSand;
                rockWall1 = WallID.SandstoneBrick;
                rockWall2 = WallID.Sandstone;
                snowType = TileID.Stone;
                iceType = TileID.Ash;
                snowWall = WallID.SandstoneBrick;
                iceWall = WallID.Dirt;
            }

            if (CheckForTile(position.X, position.Y, TileID.JungleGrass, 25) || CheckForTile(position.X, position.Y, TileID.Mud, 25))
            {
                soilType = TileID.Mudstone;
                stoneType = TileID.Mud;
                soilWall1 = WallID.MudstoneBrick;
                soilWall2 = WallID.MudUnsafe;
                snowType = TileID.Stone;
                iceType = TileID.Mudstone;
                snowWall = WallID.MudstoneBrick;
                iceWall = rockWall1;
                if (Main.rand.NextBool(2)) liquidType = LiquidID.Honey;
            }

            if (CheckForTile(position.X, position.Y, TileID.SnowBlock, 25))
            {
                soilType = TileID.SnowBlock;
                stoneType = TileID.IceBlock;
                soilWall1 = WallID.IceUnsafe;
                soilWall2 = WallID.SnowWallUnsafe;
                rockWall1 = WallID.IceBrick;
                rockWall2 = WallID.SnowWallUnsafe;
                liquidType = LiquidID.Water;
            }

            for (int i = -mountainWidth / 2; i < mountainWidth / 2; i++)
            {
                for (int j = -mountainHeight; j < 50; j++)
                {
                    int rI = i + position.X;
                    int rJ = j + position.Y;

                    int targetJ = -(int)((1f - Math.Abs(i / (mountainWidth / 2f))) * mountainHeight) + (int)(Math.Sin(i * 0.1f) * 40f * mountainNoise.GetNoise(i * 0.5f, 100f) + Math.Sign(mountainNoise.GetNoise(i * 0.5f, 100f)) * 0.35f);

                    if (j > targetJ)
                    {
                        bool canPlace = false;
                        if (j > -mountainHeight + mountainHeight / 4)
                        {
                            canPlace = true;
                        }
                        else if ((topNoise.GetNoise(i * 5f, j * 5f) + 1f) / 2f < ((j + mountainHeight) / (mountainHeight / 4f)))
                        {
                            canPlace = true;
                        }

                        if (canPlace)
                        {
                            bool shouldBeSnow = j < -mountainHeight + mountainHeight / 4;
                            if (snowNoise.GetNoise(i * 5f, j * 5f) < 0f && j > -mountainHeight + mountainHeight / 4 - 10)
                            {
                                shouldBeSnow = false;
                            }
                            float caveNoiseMult = 2f;
                            float caveNoiseReq = 0.7f;
                            if (j > targetJ + (int)(50 * Math.Abs(mountainNoise.GetNoise(i * 2.5f, j * 2.5f))))
                            {
                                if (caveNoise.GetNoise(i * caveNoiseMult, j * caveNoiseMult) < caveNoiseReq)
                                {
                                    if (oreNoise1.GetNoise(i * 5f, j * 5f) > 0.55f)
                                    {
                                        oresToPlace.Add(new Tuple<Point, int>(new Point(rI, rJ), WorldGen.SavedOreTiers.Iron));
                                    }
                                    else if (oreNoise2.GetNoise(i * 5f, j * 5f) > 0.62f)
                                    {
                                        oresToPlace.Add(new Tuple<Point, int>(new Point(rI, rJ), WorldGen.SavedOreTiers.Silver));
                                        //WorldGen.PlaceTile(rI, rJ, WorldGen.SavedOreTiers.Silver, true, false);
                                    }
                                    else if (oreNoise3.GetNoise(i * 5f, j * 5f) > 0.69f)
                                    {
                                        oresToPlace.Add(new Tuple<Point, int>(new Point(rI, rJ), WorldGen.SavedOreTiers.Silver));
                                        //WorldGen.PlaceTile(rI, rJ, WorldGen.SavedOreTiers.Gold, true, false);
                                    }
                                    else
                                    {
                                        int type = stoneType;
                                        if (shouldBeSnow) type = snowType;
                                        if (dirtNoise.GetNoise(i * 5f, j * 5f) > 0.5f)
                                        {
                                            type = soilType;
                                            if (type == TileID.Mud && WispUtils.TileCanBeGrass(rI, rJ)) type = TileID.JungleGrass;
                                            if (shouldBeSnow) type = iceType;
                                        }
                                        WorldGen.PlaceTile(rI, rJ, type, true, false);
                                    }
                                }

                                if (liquidNoise1.GetNoise(i * 5f, j * 5f) > 0.75f)
                                {
                                    WorldGen.PlaceLiquid(rI, rJ, (byte)liquidType, 255);
                                }
                            }

                            if ((j > targetJ + (int)(50 * wallNoise1.GetNoise(i * 3f, j * 3f)) && wallNoise3.GetNoise(i * 2.5f, j * 2.5f) < 0.65f) || j > targetJ + mountainHeight / 4.5f)
                            {
                                if (caveNoise.GetNoise(i * caveNoiseMult, (j + 4) * caveNoiseMult) < caveNoiseReq - 0.05f)
                                {
                                    if (wallNoise2.GetNoise(i * 3f + 1000f, j * 3f + 1000f) > 0f)
                                    {
                                        if (!shouldBeSnow) WorldGen.PlaceWall(rI, rJ, soilWall1);
                                        else WorldGen.PlaceWall(rI, rJ, snowWall);
                                    }
                                    else
                                    {
                                        if (!shouldBeSnow) WorldGen.PlaceWall(rI, rJ, soilWall2);
                                        else WorldGen.PlaceWall(rI, rJ, iceWall);
                                    }
                                }
                                else
                                {
                                    if (wallNoise2.GetNoise(i * 5f, j * 5f) > 0f)
                                    {
                                        if (!shouldBeSnow) WorldGen.PlaceWall(rI, rJ, rockWall1);
                                        else WorldGen.PlaceWall(rI, rJ, snowWall);
                                    }
                                    else
                                    {
                                        if (!shouldBeSnow) WorldGen.PlaceWall(rI, rJ, rockWall2);
                                        else WorldGen.PlaceWall(rI, rJ, iceWall);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Special Cave Shapes

        void FissuresFromHell()
        {
            int amount = Main.maxTilesX / 1000;

            for (int a = 0; a < amount; a++)
            {
                FastNoiseLite noise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
                noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
                noise.SetFrequency(0.01f);
                noise.SetFractalOctaves(5);
                noise.SetFractalLacunarity(2f);
                noise.SetFractalGain(0.5f);
                noise.SetFractalPingPongStrength(2f);

                Point position = new Point(WorldGen.genRand.Next(Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 400 - Main.maxTilesY / 12, Main.maxTilesY));
                int overallLength = Main.maxTilesY / 4;
                int defWidth = WorldGen.genRand.Next(10, 20);
                for (int i = position.X - overallLength / 2; i < position.X + overallLength / 2; i++)
                {
                    if (i > 0 && i < Main.maxTilesX)
                    {
                        for (int j = position.Y - overallLength / 2; j < position.Y + overallLength / 2; j++)
                        {
                            if (j > 0 && j < Main.maxTilesY)
                            {
                                int iAdd = (int)(Math.Sin(j / 20f) * noise.GetNoise(j * 0.5f, 0f) * (defWidth / 2f + 5));
                                int width = (int)((float)(overallLength / 2f - Math.Abs(j - position.Y)) / overallLength * 2f * defWidth);
                                //height += (int)(5f * noise.GetNoise(i * 10f, 1000f));

                                if (Math.Abs(i - position.X) <= width)
                                {
                                    WorldGen.KillTile(i + iAdd, j);
                                }
                            }
                        }
                    }
                }
            }
        }

        void SineCaves()
        {
            int amount = Main.maxTilesX / 400;

            for (int a = 0; a < amount; a++)
            {
                FastNoiseLite noise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
                noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
                noise.SetFrequency(0.01f);
                noise.SetFractalOctaves(5);
                noise.SetFractalLacunarity(2f);
                noise.SetFractalGain(0.5f);
                noise.SetFractalPingPongStrength(2f);

                Point position = new Point(WorldGen.genRand.Next(Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY));
                int overallLength = WorldGen.genRand.Next(500, 1500);
                int defHeight = WorldGen.genRand.Next(10, 20);
                for (int i = position.X - overallLength / 2; i < position.X + overallLength / 2; i++)
                {
                    if (i > 0 && i < Main.maxTilesX)
                    {
                        for (int j = position.Y - overallLength / 2; j < position.Y + overallLength / 2; j++)
                        {
                            if (j > 0 && j < Main.maxTilesY)
                            {
                                int jAdd = (int)(Math.Sin(i / 20f) * noise.GetNoise(i * 0.5f, 0f) * (defHeight / 2f + 5) * 2f);
                                int height = (int)((float)(overallLength / 2f - Math.Abs(i - position.X)) / overallLength * 2f * defHeight);
                                //height += (int)(5f * noise.GetNoise(i * 10f, 1000f));

                                if (Math.Abs(j - position.Y) <= height)
                                {
                                    WorldGen.KillTile(i, j + jAdd);
                                }
                            }
                        }
                    }
                }
            }
        }

        void LargeCaves()
        {
            int amount = Main.maxTilesX / 300;

            for (int a = 0; a < amount; a++)
            {
                FastNoiseLite noise = new FastNoiseLite(WorldGen.genRand.Next(5000, 10000));
                noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
                noise.SetFrequency(0.01f);
                noise.SetFractalOctaves(5);
                noise.SetFractalLacunarity(2f);
                noise.SetFractalGain(0.5f);
                noise.SetFractalPingPongStrength(2f);

                Point position = new Point(WorldGen.genRand.Next(Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY));
                float maxDistance = WorldGen.genRand.Next(30, 65);
                for (int i = position.X - 100; i < position.X + 100; i++)
                {
                    if (i > 0 && i < Main.maxTilesX)
                    {
                        for (int j = position.Y - 100; j < position.Y + 100; j++)
                        {
                            if (j > 0 && j < Main.maxTilesY)
                            {
                                Point tilePos = new (i, j);
                                float multAmount = WorldGen.genRand.Next(97, 104);
                                Vector2 rotationPosition = position.ToVector2().DirectionTo(tilePos.ToVector2()) * multAmount;
                                float distance = MathHelper.Lerp((noise.GetNoise(rotationPosition.X, rotationPosition.Y) + 1f) / 2f, 1f, 0.5f) * maxDistance;

                                if (tilePos.ToVector2().Distance(position.ToVector2()) < distance)
                                {
                                    WorldGen.KillTile(i, j);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Misc Surface Stuff
        public void Fulgarite()
        {
            for (int it = 0; it < Main.maxTilesX * 0.005f; it++)
            {
                Vector2 currentPosition = new Vector2();

                List<int> sandTiles = new List<int>()
                {
                    TileID.Sand,
                    TileID.Sandstone,
                    TileID.HardenedSand,
                    TileID.Dirt //WHY DOES THIS MAKE IT WORK I'M SO CONFUSED BUT WHATEVER
                };

                bool foundFulgaritePosition = false;
                int tries = 0;
                while (!foundFulgaritePosition)
                {
                    currentPosition = new Vector2(WorldGen.genRand.Next(150, Main.maxTilesX - 150), 100);
                    while (!(Main.tileSolid[Main.tile[(int)currentPosition.X, (int)currentPosition.Y].TileType] || currentPosition.Y >= Main.maxTilesY * 0.75f || sandTiles.Contains(Main.tile[(int)currentPosition.X, (int)currentPosition.Y].TileType)))
                    {
                        currentPosition.Y++;
                    }
                    if (sandTiles.Contains(Main.tile[(int)currentPosition.X, (int)currentPosition.Y].TileType))
                    {
                        foundFulgaritePosition = true;
                        break;
                    }
                    //tries++;
                    //if (tries > 1000)
                    //{
                    //    break;
                    //}
                }
                while (!WorldUtils.Find(currentPosition.ToPoint(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                    {
        new Conditions.IsSolid()
                    }), out _))
                {
                    currentPosition.Y++;
                }

                if (foundFulgaritePosition)
                {
                    for (int j = 0; j < WorldGen.genRand.Next(10, 25); j++)
                    {
                        int currentJ = (int)currentPosition.Y + j;
                        for (int i = -1; i <= 1; i++)
                        {
                            int currentI = (int)currentPosition.X + i;
                            if (Main.tile[currentI, currentJ] != null)
                            {
                                Tile tile = Main.tile[currentI, currentJ];
                                if (tile.TileType == TileID.Sand)
                                {
                                    if (WorldGen.genRand.NextBool(7, 10))
                                    {
                                        tile.TileType = (ushort)ModContent.TileType<Tiles.Fulgarite>();
                                    }
                                    else if (WorldGen.genRand.NextBool())
                                    {
                                        tile.TileType = TileID.Glass;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
