using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Wisplantern.Globals.GItems
{
    class AggravatingItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public float manipulativePower = 0f;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (manipulativePower > 0f)
            {
                int index = tooltips.FindIndex(x => x.Name == "Damage" && x.Mod == "Terraria");
                if (index != -1)
                {
                    TooltipLine powerLine = new TooltipLine(Mod, "ManipulativePower", Math.Round(manipulativePower * 100f, 1).ToString() + "% manipulative power");
                    tooltips.Insert(index + 1, powerLine);
                }
                //index = tooltips.FindIndex(x => x.Name == "ItemName" && x.Mod == "Terraria");
                //if (index != -1)
                //{
                //    TooltipLine manipLine = new TooltipLine(Mod, "ManipulativeWeapon", "-Manipulator Class-");
                //    manipLine.OverrideColor = Color.MediumPurple;
                //    tooltips.Insert(index + 1, manipLine);
                //}
            }
        }
    }
}
