using Wisplantern.Dusts;

namespace Wisplantern.Tiles
{
    class Pyrite : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileShine[Type] = (int)(Main.tileShine[TileID.Platinum] * 1.2f);
            //Main.tileLighted[Type] = true;

            HitSound = SoundID.Tink;

            MinPick = 35;
            MineResist = 2f;
            DustType = ModContent.DustType<PyriteDust>();
            AddMapEntry(new Color(227, 225, 200), CreateMapEntryName());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            //r = 187 / 5000f;
            //g = 238 / 5000f;
            //b = 206 / 5000f;
        }
    }
}
