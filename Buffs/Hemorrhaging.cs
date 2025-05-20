namespace Wisplantern.Buffs
{
    class Hemorrhaging : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, npc.velocity.X, npc.velocity.Y);
        }
    }
}