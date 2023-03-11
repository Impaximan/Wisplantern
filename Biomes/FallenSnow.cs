using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace Wisplantern.Biomes
{
    class FallenSnow : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
            {
                if (Filters.Scene["Wisplantern:WinterShader"].Active)
                {

                }
                else
                {
                    Filters.Scene.Activate("Wisplantern:WinterShader");
                }
            }
            else if (Filters.Scene["Wisplantern:WinterShader"].Active)
            {
                Filters.Scene["Wisplantern:WinterShader"].Deactivate();
            }
        }

        public override int Music => MusicID.Snow;

        public override bool IsBiomeActive(Player player)
        {
            int snowCount = 0;
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

                            if (tile.TileType == ModContent.TileType<Tiles.SnowGrass>())
                            {
                                snowCount++;
                                if (snowCount >= 50)
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
