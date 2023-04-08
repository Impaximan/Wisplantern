using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Wisplantern.Buffs
{
    class Hyperspeed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hyperspeed");
            // Description.SetDefault("Greatly increased mining speed");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.pickSpeed *= 0.3f;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            if (player.buffTime[buffIndex] < 900 && time <= 900)
            {
                player.buffTime[buffIndex] += time;
                if (player.buffTime[buffIndex] > 900)
                {
                    player.buffTime[buffIndex] = 900;
                }
                return true;
            }
            return false;
        }
    }
}