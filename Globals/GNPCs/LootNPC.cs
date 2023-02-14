using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace Wisplantern.Globals.GNPCs
{
    class LootNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (NPCID.Sets.Skeletons[npc.type])
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Wisplantern.Items.Weapons.Manipulative.SoultouchingStaff>(), 50));
            }

            if (NPCID.Sets.Zombies[npc.type])
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Wisplantern.Items.Weapons.Manipulative.SoultouchingStaff>(), 75));
            }

            if (NPCID.Sets.DemonEyes[npc.type])
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Wisplantern.Items.Weapons.Manipulative.SoultouchingStaff>(), 100));
            }
        }
    }
}