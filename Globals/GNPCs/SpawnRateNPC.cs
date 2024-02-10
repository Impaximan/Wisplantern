using System.Collections.Generic;

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

            if (spawnInfo.Player.InModBiome<Biomes.FallenSnow>() && spawnInfo.Player.ZoneOverworldHeight)
            {
                if (Main.dayTime)
                {
                    pool.Add(NPCID.IceSlime, 0.5f);
                }
                else
                {
                    pool.Add(NPCID.ZombieEskimo, 0.8f);
                    pool.Add(NPCID.ArmedZombieEskimo, 0.25f);
                }
            }
        }
    }
}
