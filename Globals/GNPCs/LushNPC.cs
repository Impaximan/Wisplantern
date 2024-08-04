namespace Wisplantern.Globals.GNPCs
{
    class LushNPC : GlobalNPC
    {
        public override bool InstancePerEntity => false;

        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (target.type == ModContent.NPCType<NPCs.Critters.Bugs.Lushfly>())
            {
                return false;
            }
            return true;
        }
    }
}
