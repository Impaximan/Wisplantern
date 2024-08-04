
namespace Wisplantern.Globals.GItems
{
    public class HuntingItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public bool huntingWeapon = false;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (huntingWeapon)
            {
                if (Wisplantern.classTags)
                {
                    int index = tooltips.FindIndex(x => x.Name == "ItemName" && x.Mod == "Terraria");
                    if (index != -1)
                    {
                        TooltipLine manipLine = new(Mod, "HuntingWeapon", "-Hunting Weapon-");
                        manipLine.OverrideColor = Color.Brown;
                        tooltips.Insert(index + 1, manipLine);
                    }
                }

                TooltipLine huntingDesc = new(Mod, "HuntingDescription", "Enemies killed with this weapon are significantly more likely to drop food");
                huntingDesc.IsModifier = true;
                tooltips.Add(huntingDesc);
            }
        }
    }
}
