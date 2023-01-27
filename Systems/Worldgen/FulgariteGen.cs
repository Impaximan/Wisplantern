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
    class FulgariteGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Full Desert"));
            tasks.Insert(genIndex + 1, new PassLegacy("Fulgarite", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Striking The Earth";

                Fulgarite();
            }));
        }

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
    }
}
