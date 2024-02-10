namespace Wisplantern.Tiles
{
    class Hyperstone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            Main.tileLighted[Type] = true;

            HitSound = SoundID.Item29;

            MinPick = 0;
            MineResist = 0f;
            DustType = DustID.BlueTorch;
            AddMapEntry(new Color(187, 206, 238));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 187 / 400f;
            g = 206 / 400f;
            b = 238 / 400f;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!effectOnly)
            {
                if (Main.netMode != NetmodeID.Server && !Main.gameMenu)
                {
                    Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
                    player.itemTime /= 3;
                    player.AddBuff(ModContent.BuffType<Buffs.Hyperspeed>(), 30);
                }
                fail = false;
            }
        }
    }
}
