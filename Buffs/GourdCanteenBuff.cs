namespace Wisplantern.Buffs
{
    class GourdCanteenBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 1f;
            player.maxRunSpeed *= 1.3f;
            player.lifeRegen += 12;
        }
    }
}