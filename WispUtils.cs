using Microsoft.Xna.Framework;
using Terraria;
using Terraria.WorldBuilding;

namespace Wisplantern
{
    public static class WispUtils
    {
        public static bool TileCanBeLush(int i, int j)
        {
            if (WorldGen.TileEmpty(i + 1, j) || !Main.tileSolid[Main.tile[i + 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j) || !Main.tileSolid[Main.tile[i - 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j + 1) || !Main.tileSolid[Main.tile[i, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j - 1) || !Main.tileSolid[Main.tile[i, j - 1].TileType])
            {
                return true;
            }
            //else if (WorldGen.TileEmpty(i + 1, j + 1) || !Main.tileSolid[Main.tile[i + 1, j + 1].TileType])
            //{
            //    return true;
            //}
            //else if (WorldGen.TileEmpty(i + 1, j - 1) || !Main.tileSolid[Main.tile[i + 1, j - 1].TileType])
            //{
            //    return true;
            //}
            //else if (WorldGen.TileEmpty(i - 1, j + 1) || !Main.tileSolid[Main.tile[i - 1, j + 1].TileType])
            //{
            //    return true;
            //}
            //else if (WorldGen.TileEmpty(i - 1, j - 1) || !Main.tileSolid[Main.tile[i - 1, j - 1].TileType])
            //{
            //    return true;
            //}

            return false;
        }

        public static Vector2 FindGroundUnder(this Vector2 position)
        {
            Vector2 returned = position;
            while (!WorldUtils.Find(returned.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                returned.Y++;
            }

            return returned;
        }

        public static Vector2 FindGroundUnder(this Vector2 position, int type)
        {
            Vector2 returned = position;

            int i = 5000;
            while (Main.tile[(int)(returned.X / 16), (int)(returned.Y / 16)].TileType != type)
            {
                returned.Y++;

                i--;
                if (i <= 0)
                {
                    return Vector2.Zero;
                }
            }

            return returned;
        }

        public static Vector2 FindCeilingAbove(this Vector2 position)
        {
            Vector2 returned = position;
            while (!WorldUtils.Find(returned.ToTileCoordinates(), Searches.Chain(new Searches.Up(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                returned.Y--;
            }

            return returned;
        }

        public static Vector2 FindCeilingAbove(this Vector2 position, int type)
        {
            Vector2 returned = position;

            int i = 5000;
            while (Main.tile[(int)(returned.X / 16), (int)(returned.Y / 16)].TileType != type)
            {
                returned.Y--;

                i--;
                if (i <= 0)
                {
                    return Vector2.Zero;
                }
            }

            return returned;
        }
    }
}
