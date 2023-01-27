using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;

namespace Wisplantern.Tiles
{
    public class LushGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            //Main.tileLighted[Type] = true;
            //Main.treeStyle[Type] = 0;
            HitSound = SoundID.Dig;
            TileID.Sets.Grass[Type] = true;
            //TileID.Sets.GrassSpecial[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            //Main.soundDig[Type] =  21;

            Main.tileMerge[TileID.Grass][Type] = true;
            Main.tileMerge[TileID.Dirt][Type] = true;

            Main.tileMerge[Type][TileID.SnowBlock] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[Type][TileID.WoodBlock] = true;
            Main.tileMerge[Type][TileID.Grass] = true;

            //Main.treeStyle[Type] = 4;

            DustType = DustID.Grass;
            MinPick = 0;
            MineResist = 0f;
            AddMapEntry(new Color(157, 218, 164));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!effectOnly)
            {
                effectOnly = true;
                noItem = false;
                Main.tile[i, j].TileType = TileID.Dirt;
                SoundEngine.PlaySound(SoundID.Grass);
            }
        }

        public override void WalkDust(ref int DustType, ref bool makeDust, ref Color color)
        {
            DustType = DustID.Grass;
            makeDust = true;
        }

        public override void RandomUpdate(int i, int j)
        {

        }

        public override bool HasWalkDust()
        {
            return true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}