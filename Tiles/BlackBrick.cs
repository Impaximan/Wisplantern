using Terraria.Audio;

namespace Wisplantern.Tiles
{
    class BlackBrick : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<IgneousStone>()] = true;


            SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/StoneHit2");
            style.PitchVariance = 0.25f;
            HitSound = style;

            MinPick = 0;
            MineResist = 1f;
            DustType = DustID.Stone;
            //ItemDrop = ModContent.ItemType<Items.Placeable.Blocks.Depthrock>();
            AddMapEntry(new Color(83, 72, 69));
        }
    }
}
