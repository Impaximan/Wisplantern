﻿using Terraria.Audio;

namespace Wisplantern.Tiles
{
    class Depthrock : ModTile
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

            SoundStyle style = new("Wisplantern/Sounds/Effects/StoneHit2")
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
