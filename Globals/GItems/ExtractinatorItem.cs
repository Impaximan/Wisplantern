using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.ID;
using Terraria.Utilities;

namespace Wisplantern.Globals.GItems
{
    class ExtractinatorItem : GlobalItem
    {

        public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
        {
            if (extractType == 0)
            {
                if (Main.rand.NextBool(50))
                {
                    resultType = ModContent.ItemType<Items.Equipable.Accessories.Flint>();
                    resultStack = 1;
                }
            }
        }
    }
}
