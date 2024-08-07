using Terraria.GameContent.ItemDropRules;
using Wisplantern.Globals.GItems;


namespace Wisplantern.DropConditions
{
    public class UsedHuntingWeapon : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (info.player == null || info.player.HeldItem == null)
            {
                return false;
            }

            return info.player.HeldItem.TryGetGlobalItem(out HuntingItem huntingItem) && huntingItem.huntingWeapon;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "[c/bf6313:Drop chance when using hunting weapons]";
        }
    }
}
