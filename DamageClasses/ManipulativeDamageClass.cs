namespace Wisplantern.DamageClasses
{
    class ManipulativeDamageClass : DamageClass
    {
        public override bool UseStandardCritCalcs => true;

        public override bool GetPrefixInheritance(DamageClass damageClass)
        {
            if (damageClass == DamageClass.Generic)
            {
                return true;
            }

            return false;
        }
    }
}
