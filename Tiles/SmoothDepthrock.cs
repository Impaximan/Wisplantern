using Terraria.Audio;

namespace Wisplantern.Tiles
{
    class SmoothDepthrock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            Main.tileMerge[Type] = Main.tileMerge[TileID.MarbleBlock];

            SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/StoneHit2")
            {
                PitchVariance = 0.25f
            };
            style.Pitch += 0.15f;
            HitSound = style;

            MinPick = 0;
            MineResist = 1f;
            DustType = DustID.Stone;
            //ItemDrop = ModContent.ItemType<Items.Placeable.Blocks.Depthrock>();
            AddMapEntry(new Color(140, 118, 101));
        }
    }
}
