using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Globals.GNPCs
{
    class SpawnRateNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (Systems.Events.CalmNight.calmNight)
            {
                spawnRate *= 4;
                maxSpawns /= 4;
            }
        }
    }
}
