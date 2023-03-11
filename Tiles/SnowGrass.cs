using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;

namespace Wisplantern.Tiles
{
    public class SnowGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            HitSound = SoundID.Dig;
            TileID.Sets.Grass[Type] = true;
            //TileID.Sets.GrassSpecial[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            //Main.soundDig[Type] =  21;

            Main.tileMerge[TileID.Grass][Type] = true;
            Main.tileMerge[TileID.Dirt][Type] = true;

            Main.tileMerge[Type][TileID.SnowBlock] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[Type][TileID.WoodBlock] = true;
            Main.tileMerge[Type][TileID.Grass] = true;

            //Main.treeStyle[Type] = 4;

            DustType = DustID.Snow;
            ItemDrop = ItemID.Snowball;
            MinPick = 0;
            MineResist = 0f;
            AddMapEntry(new Color(240, 245, 255));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            effectOnly = true;
            noItem = false;
            Main.tile[i, j].TileType = TileID.Grass;
            Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16f, ItemDrop, Main.rand.Next(1, 3));
            SoundEngine.PlaySound(SoundID.Item48, new Vector2(i, j) * 16f);
        }

        public override void WalkDust(ref int DustType, ref bool makeDust, ref Color color)
        {
            DustType = DustID.SnowBlock;
            makeDust = true;
        }

        //public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        //{
        //    Tile tile = Main.tile[i, j];
        //    Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
        //    if (Main.drawToScreen)
        //    {
        //        zero = Vector2.Zero;
        //    }
        //    int height = tile.frameY == 36 ? 18 : 16;
        //    Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Tiles/Incendiary/KindlingGrass_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        //}

        public override void RandomUpdate(int i, int j)
        {
            if (!Systems.Events.Snowstorm.snowing)
            {
                if (Main.rand.NextFloat() <= 0.35f)
                {
                    Main.tile[i, j].TileType = TileID.Grass;
                }
            }
        }

        public override bool HasWalkDust()
        {
            return true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}