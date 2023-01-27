using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Wisplantern.Tiles
{
    class Moonflower : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.AllowLightInWater[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.addTile(Type);

        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Placeable.Furniture.Moonflower>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0)
            {
                float thingy = 2f;
                r = 0.49f / thingy;
                g = 0.78f / thingy;
                b = 1.00f / thingy;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int realI = i;
            if (Main.tile[i + 1, j].TileType == Type && Main.tile[i - 1, j].TileType != Type)
            {
                realI += 1;
            }
            int uniqueAnimationFrame = realI;
            uniqueAnimationFrame = uniqueAnimationFrame % 3;

            frameXOffset = uniqueAnimationFrame * 36;
        }
    }
}
