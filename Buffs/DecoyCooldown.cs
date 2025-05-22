namespace Wisplantern.Buffs
{
    class DecoyCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.buffTime[buffIndex] == 1)
                {
                    player.DoManaRechargeEffect();
                }
            }
        }
    }
}