using Microsoft.Xna.Framework;
using Terraria;
using Terraria.WorldBuilding;
using System.Threading;
using System;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Wisplantern.Globals.GItems;
using Terraria.GameContent.Creative;

namespace Wisplantern
{
    public static class WispUtils
    {
        public static void InvokeOnMainThread(Action action)
        {
            if (!AssetRepository.IsMainThread)
            {
                ManualResetEvent evt = new(false);

                Main.QueueMainThreadAction(() => {
                    action();
                    evt.Set();
                });

                evt.WaitOne();
            }
            else
                action();
        }

        public static void DoManaRechargeEffect(this Player player)
        {
            if (player.whoAmI == Main.myPlayer) SoundEngine.PlaySound(SoundID.MaxMana);
            for (int i = 0; i < 5; i++)
            {
                int num3 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                Main.dust[num3].noLight = true;
                Main.dust[num3].noGravity = true;
                Dust obj = Main.dust[num3];
                obj.velocity *= 0.5f;
            }
        }

        public static void DoBattleArtRechargeEffect(this Player player)
        {
            SoundStyle soundStyle = SoundID.MaxMana;
            soundStyle.Pitch -= 0.5f;
            if (player.whoAmI == Main.myPlayer) SoundEngine.PlaySound(soundStyle);
            for (int i = 0; i < 5; i++)
            {
                int num3 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                Main.dust[num3].noLight = true;
                Main.dust[num3].noGravity = true;
                Dust obj = Main.dust[num3];
                obj.velocity *= 0.5f;
            }
        }

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

        public static bool TileCanBeGrass(int i, int j)
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
            else if (WorldGen.TileEmpty(i + 1, j + 1) || !Main.tileSolid[Main.tile[i + 1, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j - 1) || !Main.tileSolid[Main.tile[i + 1, j - 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j + 1) || !Main.tileSolid[Main.tile[i - 1, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j - 1) || !Main.tileSolid[Main.tile[i - 1, j - 1].TileType])
            {
                return true;
            }

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

        public static void SetAsBattleArtItem(this Item item, BattleArt battleArt)
        {
            item.GetGlobalItem<BattleArtItem>().isBattleArtItem = true;
            item.GetGlobalItem<BattleArtItem>().battleArtItemBattleArt = battleArt;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[item.type] = 1;
            item.maxStack = 20;
        }
    }
}
