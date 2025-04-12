namespace Wisplantern.Buffs
{
    class GrassCutterBuff : ModBuff
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
            player.GetModPlayer<EquipmentPlayer>().zweihanderSpeed += 2f;
        }
    }
}