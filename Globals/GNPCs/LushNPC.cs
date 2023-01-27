using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Globals.GNPCs
{
    class LushNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool? CanHitNPC(NPC npc, NPC target)
        {
            if (target.type == ModContent.NPCType<NPCs.Critters.Bugs.Lushfly>())
            {
                return false;
            }
            return base.CanHitNPC(npc, target);
        }
    }
}
