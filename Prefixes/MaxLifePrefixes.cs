using System.Collections.Generic;

namespace Wisplantern.Prefixes
{
    class MaxLifePrefixItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int GetExtraLifeCount(Item item)
        {
            int prefix = item.prefix;

            if (prefix == ModContent.PrefixType<Steadfast>())
            {
                return 2;
            }

            if (prefix == ModContent.PrefixType<Hopeful>())
            {
                return 4;
            }

            if (prefix == ModContent.PrefixType<Lively>())
            {
                return 6;
            }

            if (prefix == ModContent.PrefixType<Vital>())
            {
                return 8;
            }

            return 0;
        }

        public override void UpdateEquip(Item item, Player player)
        {
            int lifeCount = GetExtraLifeCount(item);
            player.statLifeMax2 += lifeCount;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (GetExtraLifeCount(item) != 0)
            {
                TooltipLine line = new(Mod, "MaxLifePrefix", "+" + GetExtraLifeCount(item).ToString() + " max life")
                {
                    IsModifier = true
                };
                tooltips.Add(line);
            }
        }
    }

    class Steadfast : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Steadfast");
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.05f;
        }
    }

    class Hopeful : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hopeful");
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.1f;
        }
    }

    class Lively : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lively");
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.15f;
        }
    }

    class Vital : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Vital");
        }


        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.2f;
        }
    }
}
