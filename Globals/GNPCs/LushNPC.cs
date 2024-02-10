namespace Wisplantern.Globals.GNPCs
{
    class LushNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool CanHitNPC(NPC npc, NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            if (target.type == ModContent.NPCType<NPCs.Critters.Bugs.Lushfly>())
            {
                return false;
            }
            return base.CanHitNPC(npc, target);
        }
    }
}
