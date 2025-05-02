using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wisplantern.DamageClasses;
using Wisplantern.Globals.GItems;

namespace Wisplantern.Prefixes
{
    class Rlyehian : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1.09f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 1.1f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 1.15f;
            knockbackMult = 1.15f;
            useTimeMult = 1f / 1.1f;
            critBonus = 5;
        }
    }

    class Chaotic : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 0.18f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 0.9f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 0.9f;
            useTimeMult = 1f / 1.1f;
        }
    }

    class Loving : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 0.11f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 1.2f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 0.8f;
            knockbackMult = 0.7f;
        }
    }

    class Introverted : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult -= 0.34f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 0.8f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 0.95f;
            knockbackMult = 1.3f;
        }
    }

    class Charismatic : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 0.6f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 1.15f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult = 1f / 1.1f;
        }
    }

    class Charming : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 0.3f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 1.15f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult = 1f / 1.15f;
        }
    }

    class Belligerant : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult -= 0.2f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 0.8f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            shootSpeedMult = 1f / 1.1f;
            damageMult = 0.9f;
        }
    }

    class Rude : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult -= 0.5f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 0.7f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 0.9f;
        }
    }

    class Sensitive : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult -= 0.2f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 0.9f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult = 1.1f;
        }
    }

    class Harsh : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 0.2f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 0.9f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 1.1f;
            useTimeMult = 1f / 1.1f;
        }
    }

    class Clever : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 0.1f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 1.2f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult = 1.2f;
        }
    }

    class Convincing : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item)
        {
            return item.DamageType == ModContent.GetInstance<ManipulativeDamageClass>();
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 0.3f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower *= 1.15f;
            base.Apply(item);
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult = 1.1f;
            damageMult = 0.95f;
        }
    }
}
