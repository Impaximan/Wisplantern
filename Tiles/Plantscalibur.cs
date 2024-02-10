using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Wisplantern.Tiles
{
    class Plantscalibur : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.AllowLightInWater[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
            TileObjectData.addTile(Type);

        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Weapons.Magic.Staffs.Plantscalibur>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0)
            {
                float thingy = 1.3f;
                r = 0.59f / thingy;
                g = 0.85f / thingy;
                b = 1.00f / thingy;
            }
        }
    }
}
