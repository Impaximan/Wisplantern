using Terraria.GameContent.ItemDropRules;

namespace Wisplantern.Globals.GNPCs
{
    class LootNPC : GlobalNPC
    {
        public override bool InstancePerEntity => false;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            List<IItemDropRule> dropRules = npcLoot.Get(true);

            foreach (IItemDropRule rule in dropRules)
            {
                if (rule is ItemDropWithConditionRule itemRule)
                {
                    if (ItemID.Sets.IsFood[itemRule.itemId])
                    {
                        npcLoot.Add(ItemDropRule.ByCondition(new DropConditions.UsedHuntingWeapon(), itemRule.itemId, Math.Clamp(itemRule.chanceDenominator / 25, itemRule.chanceNumerator, 100), 1, 1, itemRule.chanceNumerator));
                    }
                }
            }
        }
    }
}