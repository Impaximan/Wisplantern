namespace Wisplantern.Globals.GTiles
{
    class LushTile : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
        }

        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            if (i < Main.maxTilesX - 1) DestroyBadGrass(i + 1, j);
            if (i > 0) DestroyBadGrass(i - 1, j);
            if (j < Main.maxTilesY - 1) DestroyBadGrass(i, j + 1);
            if (j > 0) DestroyBadGrass(i, j - 1);
        }

        public void DestroyBadGrass(int i, int j)
        {
            if (Main.tile[i, j].TileType == ModContent.TileType<Tiles.LushGrass>())
            {
                if (!WispUtils.TileCanBeLush(i, j))
                {
                    Main.tile[i, j].TileType = TileID.Grass;
                }
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (i < Main.maxTilesX - 1) DestroyBadGrass(i + 1, j);
            if (i > 0) DestroyBadGrass(i - 1, j);
            if (j < Main.maxTilesY - 1) DestroyBadGrass(i, j + 1);
            if (j > 0) DestroyBadGrass(i, j - 1);
            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }
    }
}
