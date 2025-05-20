using Terraria.Audio;
using Wisplantern.Buffs;
using Wisplantern.Globals.GNPCs;
using Wisplantern.Items.Equipable.Accessories;

namespace Wisplantern.ModPlayers
{
    public class EquipmentPlayer : ModPlayer
    {
        /// <summary>
        /// A list containing the ID of every Wisplantern accessory currently equipped.
        /// </summary>
        public List<int> accessoryEffects = new();

        public List<Projectile> pandemoniumSouls = new();
        public bool firePandemoniumSouls = false;

        public float zweihanderSpeed = 1f;

        public override void ResetEffects()
        {
            accessoryEffects.Clear();
            zweihanderSpeed = 1f;
        }

        const float critDamageMult = 1.3f;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.AccessoryActive<FocusingCrystal>())
            {
                modifiers.CritDamage *= critDamageMult;
            }

            if (target.HasBuff<Marked>())
            {
                modifiers.FinalDamage *= 1.3f;
                modifiers.HideCombatText();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.HasBuff<Marked>() && Main.netMode != NetmodeID.Server)
            {
                CombatText.NewText(target.getRect(), hit.Crit ? Color.Red : Color.Crimson, hit.Damage, hit.Crit);

                SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/DamageTick2");
                style.MaxInstances = 10;
                style.PitchVariance = 0.3f;
                style.Volume *= 0.5f;
                style.Pitch -= 0.3f;
                style.SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest;

                SoundEngine.PlaySound(style);
            }

            if (Player.AccessoryActive<DeathMark>())
            {
                if (Main.rand.NextBool(15))
                {
                    target.AddBuff(ModContent.BuffType<Hemorrhaging>(), 60 * 5);
                }

                int num = 0;

                foreach (int type in target.buffType)
                {
                    if (type > 0 && type != ModContent.BuffType<Marked>() && Main.debuff[type] && target.buffTime[target.FindBuffIndex(type)] > 0)
                    {
                        num++;
                    }
                }

                if (target.TryGetGlobalNPC(out InfightingNPC iNPC))
                {
                    if (iNPC.aggravated)
                    {
                        num++;
                    }
                }

                if (num >= 3)
                {
                    target.AddBuff(ModContent.BuffType<Marked>(), 60 * 10);
                }
            }
        }

        public override void UpdateDead()
        {
            pandemoniumSouls.Clear();
        }

        int pSoulCounter = 0;
        public override void PostUpdateMiscEffects()
        {
            if (firePandemoniumSouls && Player.whoAmI == Main.myPlayer)
            {
                pSoulCounter++;
                if (pSoulCounter >= 10)
                {
                    pSoulCounter = 0;

                    EndPSoul:

                    if (pandemoniumSouls.Count <= 0)
                    {
                        firePandemoniumSouls = false;
                    }
                    else
                    {
                        while (pandemoniumSouls.Count > 0 && (pandemoniumSouls[0] == null || !pandemoniumSouls[0].active))
                        {
                            pandemoniumSouls.RemoveAt(0);
                        }

                        if (pandemoniumSouls.Count <= 0)
                        {
                            goto EndPSoul;
                        }

                        pandemoniumSouls[0].ai[0] = 1;
                        pandemoniumSouls[0].velocity = pandemoniumSouls[0].DirectionTo(Main.MouseWorld) * 15f;
                        pandemoniumSouls[0].netUpdate = true;

                        pandemoniumSouls.RemoveAt(0);
                    }
                }
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
