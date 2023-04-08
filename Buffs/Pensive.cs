using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Wisplantern.Buffs
{
    class Pensive : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pensive");
            // Description.SetDefault("Max stats increased and monster spawns reduced");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 += 40;
            player.statManaMax2 += 40;
        }
    }

    class PensiveNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.HasBuff<Pensive>())
            {
                spawnRate += 100;
            }
        }
    }
}