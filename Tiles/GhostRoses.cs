using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Wisplantern.Tiles
{
    class GhostRose_1 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.AllowLightInWater[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
            TileObjectData.addTile(Type);

            HitSound = SoundID.Grass;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float thingy = 10f;
            r = 1f / thingy;
            g = 1f / thingy;
            b = 1f / thingy;
        }
    }

    class GhostRose_2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.AllowLightInWater[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.addTile(Type);

            HitSound = SoundID.Grass;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float thingy = 20f;
            r = 1f / thingy;
            g = 0.8f / thingy;
            b = 1f / thingy;
        }
    }
}
