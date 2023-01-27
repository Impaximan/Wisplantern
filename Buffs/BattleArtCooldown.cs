using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Wisplantern.Buffs
{
    class BattleArtCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Battle Art Cooldown");
            Description.SetDefault("You must wait to use another battle art");
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