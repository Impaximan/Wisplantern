using Terraria.Audio;

namespace Wisplantern.Tiles
{
    class IgneousStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[TileID.Stone][Type] = true;
            Main.tileMerge[Type][TileID.Mud] = true;
            Main.tileMerge[TileID.Mud][Type] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            Main.tileMerge[TileID.Ash][Type] = true;
            Main.tileMerge[Type][TileID.ClayBlock] = true;
            Main.tileMerge[TileID.ClayBlock][Type] = true;
            Main.tileMerge[Type][ModContent.TileType<Depthrock>()] = true;
            Main.tileMerge[ModContent.TileType<Depthrock>()][Type] = true;

            SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/StoneHit3")
            {
                PitchVariance = 0.5f,
                Pitch = 0.3f,
                Volume = 2f,
            };
            HitSound = style;

            MinPick = 0;
            MineResist = 1f;
            DustType = DustID.Stone;
            //ItemDrop = ModContent.ItemType<Items.Placeable.Blocks.IgneousStone>();
            AddMapEntry(new Color(113, 107, 105));
        }
    }
}

