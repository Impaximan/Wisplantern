using Wisplantern.Buffs;

namespace Wisplantern.Globals.GNPCs
{
    class MiscNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (target.type == ModContent.NPCType<NPCs.Critters.Bugs.Lushfly>())
            {
                return false;
            }
            return true;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff<Hemorrhaging>())
            {
                npc.lifeRegen -= 16;
            }
        }
    }
}
