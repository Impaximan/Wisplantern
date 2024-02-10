namespace Wisplantern.Biomes
{
    class LushCave : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.None;

        public override bool IsBiomeActive(Player player)
        {
            int lushCount = 0;
            for (int i = -60; i <= 60; i++)
            {
                for (int j = -60; j <= 60; j++)
                {
                    int trueI = (int)player.Center.X / 16 + i;
                    int trueJ = (int)player.Center.Y / 16 + j;

                    if (trueI > 0 && trueJ > 0 && trueI < Main.maxTilesX && trueJ < Main.maxTilesY)
                    {
                        if (Main.tile[trueI, trueJ] != null)
                        {
                            Tile tile = Main.tile[trueI, trueJ];

                            if (tile.TileType == ModContent.TileType<Tiles.LushGrass>())
                            {
                                lushCount++;
                                if (lushCount >= 25)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
