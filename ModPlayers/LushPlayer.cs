using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Wisplantern.ModPlayers
{
    class LushPlayer : ModPlayer
    {
        public override void PreUpdateBuffs()
        {
            if (Systems.AuraTiles.moonflower && !Main.dayTime && Player.ZoneOverworldHeight)
            {
                Player.AddBuff(ModContent.BuffType<Buffs.Pensive>(), 2);
            }
        }

        public override void PostUpdate()
        {
            for (int i = (int)Player.Center.X / 16 - 80; i < (int)Player.Center.X / 16 + 80; i++)
            {
                for (int j = (int)Player.Center.Y / 16 - 80; j < (int)Player.Center.Y / 16 + 80; j++)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].TileType == ModContent.TileType<Tiles.GhostRose_1>() || Main.tile[i, j].TileType == ModContent.TileType<Tiles.GhostRose_2>()))
                    {
                        if (Main.rand.NextBool(200))
                        {
                            int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<Dusts.GhostRoseDust>());
                            Main.dust[dust].velocity.Y = Main.rand.NextFloat(-2f, -0.5f);
                            Main.dust[dust].velocity.X = Main.rand.NextFloat(-1f, 1f);
                        }
                    }
                }
            }
        }
    }
}