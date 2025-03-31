using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Wisplantern.Tiles
{
    class LushLantern : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.AllowLightInWater[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 1;
            TileObjectData.addTile(Type);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float thingy = 2f;
            r = 0.49f / thingy;
            g = 1.00f / thingy;
            b = 0.78f / thingy;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int realI = i;
            if (Main.tile[i + 1, j].TileType == Type)
            {
                realI += 1;
            }
            int uniqueAnimationFrame = realI;
            uniqueAnimationFrame = uniqueAnimationFrame % 4;

            frameYOffset = uniqueAnimationFrame * 18;
        }
    }
}
