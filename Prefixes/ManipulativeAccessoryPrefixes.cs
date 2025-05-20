using MonoMod.Core.Platforms;
using System.Collections.Generic;

namespace Wisplantern.Prefixes
{
    class ManipulativeAccessoryPrefixItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int GetManipulativePowerBonus(Item item)
        {
            int prefix = item.prefix;

            if (prefix == ModContent.PrefixType<Cunning>())
            {
                return 2;
            }

            if (prefix == ModContent.PrefixType<Scheming>())
            {
                return 3;
            }

            if (prefix == ModContent.PrefixType<Conniving>())
            {
                return 5;
            }

            if (prefix == ModContent.PrefixType<Diabolical>())
            {
                return 6;
            }

            return 0;
        }

        public override void UpdateEquip(Item item, Player player)
        {
            int manipulativePower = GetManipulativePowerBonus(item);
            player.GetModPlayer<ManipulativePlayer>().manipulativePower += manipulativePower / 100f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (GetManipulativePowerBonus(item) != 0)
            {
                TooltipLine line = new(Mod, "ManipulativePowerAccessoryPrefix", "+" + GetManipulativePowerBonus(item).ToString() + "% manipulative power")
                {
                    IsModifier = true
                };
                tooltips.Add(line);
            }
        }
    }

    class Cunning : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.05f;
        }
    }

    class Scheming : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.1f;
        }
    }

    class Conniving : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.15f;
        }
    }

    class Diabolical : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.2f;
        }
    }
}
