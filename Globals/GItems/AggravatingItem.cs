using System.Collections.Generic;
using System;

namespace Wisplantern.Globals.GItems
{
    class AggravatingItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public float manipulativePower = 0f;

        public int charisma = 0;

        public bool markedAsManipulative = false;

        public override bool CanUseItem(Item item, Player player)
        {
            if (charisma > 0)
            {
                ManipulativePlayer mPlayer = player.GetModPlayer<ManipulativePlayer>();
                if (mPlayer.charisma >= charisma && base.CanUseItem(item, player))
                {
                    mPlayer.charisma -= charisma;
                    return true;
                }

                return false;
            }
            else
            {
                return base.CanUseItem(item, player);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (manipulativePower > 0f)
            {
                int index = tooltips.FindIndex(x => x.Name == "Damage" && x.Mod == "Terraria");
                if (index != -1)
                {
                    TooltipLine powerLine = new(Mod, "ManipulativePower", Math.Round(manipulativePower * 100f, 1).ToString() + "% manipulative power");
                    tooltips.Insert(index + 1, powerLine);
                }
            }

            if ((item.DamageType == ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>() || markedAsManipulative) && Wisplantern.classTags)
            {
                int index = tooltips.FindIndex(x => x.Name == "ItemName" && x.Mod == "Terraria");
                if (index != -1)
                {
                    TooltipLine manipLine = new(Mod, "ManipulativeWeapon", "-Shepherd Class-");
                    manipLine.OverrideColor = Color.MediumPurple;
                    tooltips.Insert(index + 1, manipLine);
                }
            }

            if (charisma > 0)
            {
                int index = tooltips.FindIndex(x => x.Name == "Knockback" && x.Mod == "Terraria");

                if (index == -1) index = tooltips.FindIndex(x => x.Name == "Speed" && x.Mod == "Terraria");
                if (index == -1) index = tooltips.FindIndex(x => x.Name == "CritChance" && x.Mod == "Terraria");
                if (index == -1) index = tooltips.FindIndex(x => x.Name == "Damage" && x.Mod == "Terraria");
                if (index == -1) index = tooltips.FindIndex(x => x.Name == "ItemName" && x.Mod == "Terraria");

                if (index != -1)
                {
                    TooltipLine powerLine = new(Mod, "CharismaCost", "Uses " + charisma.ToString() + " charisma");
                    tooltips.Insert(index + 1, powerLine);
                }
            }
        }
    }
}
