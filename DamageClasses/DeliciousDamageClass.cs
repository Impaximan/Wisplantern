namespace Wisplantern.DamageClasses
{
    class DeliciousDamageClass : DamageClass
    {
        public override bool UseStandardCritCalcs => true;

        public override bool GetPrefixInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic)
            {
                return true;
            }

            return false;
        }
    }
}
