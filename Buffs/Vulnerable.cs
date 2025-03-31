namespace Wisplantern.Buffs
{
    class Vulnerable : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Battle Art Cooldown");
            // Description.SetDefault("You must wait to use another battle art");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }
}