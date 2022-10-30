using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Wisplantern.Tiles
{
    class Fulgarite : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;

            Main.tileMerge[Type][TileID.Glass] = true;
            Main.tileMerge[TileID.Glass][Type] = true;

            Main.tileMerge[Type][TileID.Sand] = true;
            Main.tileMerge[TileID.Sand][Type] = true;
            Main.tileMerge[Type][TileID.Sandstone] = true;
            Main.tileMerge[TileID.Sandstone][Type] = true;
            Main.tileMerge[Type][TileID.HardenedSand] = true;
            Main.tileMerge[TileID.HardenedSand][Type] = true;

            Main.tileMerge[Type][TileID.Ebonsand] = true;
            Main.tileMerge[TileID.Ebonsand][Type] = true;
            Main.tileMerge[Type][TileID.CorruptSandstone] = true;
            Main.tileMerge[TileID.CorruptSandstone][Type] = true;
            Main.tileMerge[Type][TileID.CorruptHardenedSand] = true;
            Main.tileMerge[TileID.CorruptHardenedSand][Type] = true;

            Main.tileMerge[Type][TileID.Crimsand] = true;
            Main.tileMerge[TileID.Crimsand][Type] = true;
            Main.tileMerge[Type][TileID.CrimsonSandstone] = true;
            Main.tileMerge[TileID.CrimsonSandstone][Type] = true;
            Main.tileMerge[Type][TileID.CrimsonHardenedSand] = true;
            Main.tileMerge[TileID.CrimsonHardenedSand][Type] = true;

            Main.tileMerge[Type][TileID.Pearlsand] = true;
            Main.tileMerge[TileID.Pearlsand][Type] = true;
            Main.tileMerge[Type][TileID.HallowSandstone] = true;
            Main.tileMerge[TileID.HallowSandstone][Type] = true;
            Main.tileMerge[Type][TileID.HallowHardenedSand] = true;
            Main.tileMerge[TileID.HallowHardenedSand][Type] = true;

            SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/StoneHit1");
            style.PitchVariance = 0.25f;
            style.Pitch += 0.3f;
            HitSound = style;

            MinPick = 0;
            MineResist = 1f;
            DustType = DustID.Stone;
            ItemDrop = ModContent.ItemType<Items.Placeable.Blocks.Fulgarite>();
            AddMapEntry(new Color(202, 212, 155));
        }
    }
}
