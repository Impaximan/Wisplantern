namespace Wisplantern.Tiles
{
    class PandemoniumOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileShine[Type] = (int)(Main.tileShine[TileID.Platinum] * 0.7f);
            //Main.tileLighted[Type] = true;

            HitSound = SoundID.Tink;

            MinPick = 65;
            MineResist = 2f;
            DustType = DustID.GreenTorch;
            AddMapEntry(new Color(22, 210, 105), CreateMapEntryName());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            //r = 187 / 5000f;
            //g = 238 / 5000f;
            //b = 206 / 5000f;
        }
    }
}
