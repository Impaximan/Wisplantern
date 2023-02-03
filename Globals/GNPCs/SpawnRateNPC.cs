using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Wisplantern.Globals.GNPCs
{
    class SpawnRateNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (Systems.Events.CalmNight.calmNight && player.ZoneOverworldHeight)
            {
                int num = 2;
                spawnRate *= num;
                maxSpawns /= num;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (Systems.Events.CalmNight.calmNight && spawnInfo.Player.ZoneOverworldHeight)
            {
                if (spawnInfo.Player.ZoneHallow)
                {
                    pool.Add(NPCID.LightningBug, 5f);
                }
                else
                {
                    pool.Add(NPCID.Firefly, 5f);
                }
            }
        }
    }
}
