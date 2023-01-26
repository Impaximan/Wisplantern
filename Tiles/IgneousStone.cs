using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
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

            SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/StoneHit2");
            style.PitchVariance = 0.25f;
            HitSound = style;

            MinPick = 0;
            MineResist = 1f;
            DustType = DustID.Stone;
            ItemDrop = ModContent.ItemType<Items.Placeable.Blocks.IgneousStone>();
            AddMapEntry(new Color(117, 104, 130));
        }
    }
}

