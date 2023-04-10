using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace Wisplantern.Globals.GNPCs
{
    class InfightingNPC : GlobalNPC
    {
        public bool aggravated = false;
        public float aggravation = 0f;
        public int infightDamage = 10;
        public float infightKnockback = 3f;
        public int infightCritChance = 4;
        public int infightPlayer = 0;
        public Item infightItem = null;

        /// <summary>
        /// How many infighting iframes this enemy gives other enemies on hit.
        /// </summary>
        public int infightGivenIframes = 10;

        /// <summary>
        /// How many infighting iframes this enemy has left.
        /// </summary>
        public int infightIframes = 0;

        public bool decoy = false;

        public override bool InstancePerEntity => true;

        public NPC GetNPCTarget(NPC me)
        {
            NPC target = null;
            float distance = Main.player[me.target].Distance(me.Center) * 1.5f;
            float decoyDistance = Main.player[me.target].Distance(me.Center) * 1.5f;
            bool foundDecoy = false;
            bool foundBoss = false;

            foreach (NPC npc in Main.npc)
            {
                bool prioritize = (npc.Distance(me.Center) < distance || Main.player[me.target].GetModPlayer<ModPlayers.ManipulativePlayer>().smokeBombTime > 0 || npc.boss) && !foundDecoy && !foundBoss;
                if (npc.TryGetGlobalNPC(out InfightingNPC result))
                {
                    if (result.decoy && (npc.Distance(me.Center) < decoyDistance || Main.player[me.target].GetModPlayer<ModPlayers.ManipulativePlayer>().smokeBombTime > 0)) prioritize = true;
                }
                if (npc.active && prioritize && npc.whoAmI != me.whoAmI && !npc.dontTakeDamage && !(me.aiStyle == 9 && npc.whoAmI == castAIOwner) && npc.aiStyle != 9)
                {
                    target = npc;
                    distance = npc.Distance(me.Center);

                    if (result.decoy)
                    {
                        foundDecoy = true;
                        decoyDistance = distance;
                    }

                    if (target.boss)
                    {
                        foundBoss = true;
                    }
                }
            }

            return target;
        }

        public bool CanSeeTarget(NPC me, NPC other)
        {
            if (me.noTileCollide)
            {
                return true;
            }
            else if (Collision.CanHitLine(me.Center, 1, 1, other.Center, 1, 1))
            {
                return true;
            }
            return false;
        }

        public bool AnyVisibleDecoys(NPC me)
        {
            float distance = Main.player[me.target].Distance(me.Center);

            foreach (NPC npc in Main.npc)
            {
                if (npc.TryGetGlobalNPC(out InfightingNPC result))
                {
                    if (npc.active && npc.Distance(me.Center) < distance && npc.whoAmI != me.whoAmI && !npc.dontTakeDamage && result.decoy)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        int castAIOwner = 0;
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            aggravation = 0f;
        }

        NPC targetNPC = null;
        Vector2? originalPlayerPosition = null;
        int ogTarget = 0;
        bool shouldTeleportBack = false;
        int timeUntilCountdown = 0;

        public override bool PreAI(NPC npc)
        {
            if (aggravated || AnyVisibleDecoys(npc))
            {
                if (npc.aiStyle == 1 && npc.life == npc.lifeMax && aggravated)
                {
                    npc.life--;
                }
                targetNPC = GetNPCTarget(npc);
                if (targetNPC != null && npc.target != 255)
                {
                    ogTarget = npc.target;
                    originalPlayerPosition = Main.player[ogTarget].position;
                    Main.player[ogTarget].Center = targetNPC.Center;
                    shouldTeleportBack = true;
                }
            }
            else
            {
                timeUntilCountdown = 60;
            }
            return base.PreAI(npc);
        }

        public override void OnKill(NPC npc)
        {
            if (targetNPC != null && originalPlayerPosition.HasValue && shouldTeleportBack)
            {
                Main.player[ogTarget].position = originalPlayerPosition.Value;
                shouldTeleportBack = false;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (targetNPC != null && originalPlayerPosition.HasValue && shouldTeleportBack)
            {
                Main.player[ogTarget].position = originalPlayerPosition.Value;
                shouldTeleportBack = false;
            }

            if (aggravated)
            {
                foreach (NPC target in Main.npc)
                {
                    if (target.active && target.whoAmI != npc.whoAmI && target.Hitbox.Intersects(npc.Hitbox) && target.GetGlobalNPC<InfightingNPC>().infightIframes <= 0 && !target.friendly)
                    {
                        int damage = infightDamage;
                        if (npc.SpawnedFromStatue)
                        {
                            damage = (int)(damage * 1.35f);
                        }
                        NPC.HitInfo info = target.CalculateHitInfo(damage, Math.Sign(target.Center.X - npc.Center.X), Main.rand.NextBool(infightCritChance, 100), infightKnockback, ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>(), true, Main.player[infightPlayer].luck);
                        int struckDamage = target.StrikeNPC(info);
                        Main.player[infightPlayer].addDPS(struckDamage);
                        //foreach (int buffType in npc.buffType)
                        //{
                        //    target.AddBuff(buffType, npc.buffTime[npc.FindBuffIndex(buffType)]);
                        //}
                        target.GetGlobalNPC<InfightingNPC>().infightIframes = infightGivenIframes;
                        foreach (Instanced<GlobalNPC> gNPC in target.Globals)
                        {
                            gNPC.Instance.OnHitByItem(target, Main.player[infightPlayer], infightItem, info, damage);
                        }
                    }
                }

                if (timeUntilCountdown > 0)
                {
                    timeUntilCountdown--;
                }
                else
                {
                    if (npc.SpawnedFromStatue) aggravation -= 1f / 1200f;
                    else aggravation -= 1f / 540f;

                    if (aggravation <= 0f)
                    {
                        aggravation = 0f;
                        aggravated = false;
                        CombatText.NewText(npc.getRect(), Color.MediumPurple * 0.5f, "Calmed down...", false, true);
                    }
                }
            }

            infightIframes--;
        }
    }

    class InfightProjectile : GlobalProjectile
    {
        NPC originalNPC;

        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent)
            {
                EntitySource_Parent parentSource = source as EntitySource_Parent;
                if (parentSource.Entity is NPC)
                {
                    originalNPC = parentSource.Entity as NPC;
                    if (originalNPC.GetGlobalNPC<InfightingNPC>().aggravated)
                    {
                        projectile.friendly = true;
                    }
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (originalNPC != null)
            {
                modifiers.SourceDamage.Base = originalNPC.GetGlobalNPC<InfightingNPC>().infightDamage * 2;
                if (originalNPC.SpawnedFromStatue)
                {
                    modifiers.FinalDamage *= 1.35f;
                }
            }
        }

        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (originalNPC != null && target.whoAmI == originalNPC.whoAmI)
            {
                return false;
            }
            return base.CanHitNPC(projectile, target);
        }
    }
}
