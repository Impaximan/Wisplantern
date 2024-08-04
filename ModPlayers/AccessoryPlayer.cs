using Wisplantern.Items.Equipable.Accessories;

namespace Wisplantern.ModPlayers
{
    public class AccessoryPlayer : ModPlayer
    {
        /// <summary>
        /// A list containing the ID of every Wisplantern accessory currently equipped.
        /// </summary>
        public List<int> accessoryEffects = new();

        public override void ResetEffects()
        {
            accessoryEffects.Clear();
        }

        const float critDamageMult = 1.3f;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.AccessoryActive<FocusingCrystal>())
            {
                modifiers.CritDamage *= critDamageMult;
            }
        }

        public override void PostUpdateEquips()
        {
            if (Player.AccessoryActive<WispNecklace>())
            {
                float usedPickSpeed = Player.pickSpeed;
                if (Player.HasBuff(ModContent.BuffType<Buffs.Hyperspeed>()))
                {
                    if (usedPickSpeed < 0.5f) usedPickSpeed = 0.5f;
                }
                else
                {
                    if (usedPickSpeed < 0.75f) usedPickSpeed = 0.75f;
                }
                Player.GetAttackSpeed(DamageClass.Generic) *= MathHelper.Lerp(1f / usedPickSpeed, 1f, 0.5f);
            }
        }
    }
}
