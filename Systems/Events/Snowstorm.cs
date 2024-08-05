using Terraria.ModLoader.IO;

namespace Wisplantern.Systems.Events
{
    class Snowstorm : ModSystem
    {
        public static bool snowing = false;
        bool wasRaining = false;

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("snowing", snowing);
            tag.Add("wasRaining", wasRaining);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            snowing = tag.GetBool("snowing");
            wasRaining = tag.GetBool("wasRaining");
        }

        public override void PostUpdateEverything()
        {
            if (Main.raining && !wasRaining)
            {
                wasRaining = true;
                if (Main.rand.NextBool(4) && Wisplantern.snowstorms)
                {
                    snowing = true;
                }
            }
            else if (!Main.raining && wasRaining)
            {
                if (snowing)
                {
                    snowing = false;
                }
                wasRaining = false;
            }

            if (snowing)
            {
                for (int i = 0; i < Main.maxTilesX / 5; i++)
                {
                    int x = Main.rand.Next(0, Main.maxTilesX);
                    int y = Main.rand.Next(0, Main.maxTilesY);
                    Tile tile = Main.tile[x, y];
                    if (tile.TileType == TileID.Grass && WispUtils.TileShouldBeSnow(x, y))
                    {
                        tile.TileType = (ushort)ModContent.TileType<Tiles.SnowGrass>();
                    }
                }
            }

            if (!snowing)
            {
                for (int i1 = 0; i1 < Main.maxTilesX / 10; i1++)
                {
                    int x = Main.rand.Next(0, Main.maxTilesX);
                    int y = Main.rand.Next(0, Main.maxTilesY);
                    Tile tile = Main.tile[x, y];
                    if (tile.TileType == (ushort)ModContent.TileType<Tiles.SnowGrass>())
                    {
                        tile.TileType = (ushort)TileID.Grass;
                        for (int i = x - 5; i <= x + 5; i++)
                        {
                            for (int j = y - 5; j <= y + 5; j++)
                            {
                                if (i < 0 || i >= Main.maxTilesX || j < 0 || j >= Main.maxTilesY)
                                {
                                    continue;
                                }
                                if (Main.tile[i, j].TileType == TileID.HallowedGrass)
                                {
                                    tile.TileType = (ushort)TileID.HallowedGrass;
                                    return;
                                }
                            }
                        }
                        for (int i = x - 5; i <= x + 5; i++)
                        {
                            for (int j = y - 5; j <= y + 5; j++)
                            {
                                if (i < 0 || i >= Main.maxTilesX || j < 0 || j >= Main.maxTilesY)
                                {
                                    continue;
                                }
                                if (Main.tile[i, j].TileType == TileID.CrimsonGrass)
                                {
                                    tile.TileType = (ushort)TileID.CrimsonGrass;
                                    return;
                                }
                            }
                        }
                        for (int i = x - 5; i <= x + 5; i++)
                        {
                            for (int j = y - 5; j <= y + 5; j++)
                            {
                                if (i < 0 || i >= Main.maxTilesX || j < 0 || j >= Main.maxTilesY)
                                {
                                    continue;
                                }
                                if (Main.tile[i, j].TileType == TileID.CorruptGrass)
                                {
                                    tile.TileType = (ushort)TileID.CorruptGrass;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
