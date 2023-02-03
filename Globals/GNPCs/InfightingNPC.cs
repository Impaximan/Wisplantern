using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using System;

namespace Wisplantern.Globals.GNPCs
{
    class InfightingNPC : GlobalNPC
    {
        public bool aggravated = false;
        public int infightDamage = 10;
        public float infightKnockback = 3f;
        public int infightCritChance = 4;

        /// <summary>
        /// How many infighting iframes this enemy gives other enemies on hit.
        /// </summary>
        public int infightGivenIframes = 10;

        /// <summary>
        /// How many infighting iframes this enemy has left.
        /// </summary>
        public int infightIframes = 0;

        public override bool InstancePerEntity => true;

        public NPC GetNPCTarget(NPC me)
        {
            NPC target = null;
            float distance = Main.player[me.target].Distance(me.Center);

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.Distance(me.Center) < distance && npc.whoAmI != me.whoAmI && !npc.dontTakeDamage)
                {
                    target = npc;
                    distance = npc.Distance(me.Center);
                }
            }

            return target;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (!npc.friendly)
            {
                aggravated = true;
            }
        }

        NPC targetNPC = null;
        Vector2? originalPlayerPosition = null;
        int ogTarget = 0;
        bool shouldTeleportBack = false;

        public override bool PreAI(NPC npc)
        {
            if (aggravated)
            {
                targetNPC = GetNPCTarget(npc);
                if (targetNPC != null && npc.target != 255)
                {
                    ogTarget = npc.target;
                    originalPlayerPosition = Main.player[ogTarget].position;
                    Main.player[ogTarget].Center = targetNPC.Center;
                    shouldTeleportBack = true;
                }
            }
            return base.PreAI(npc);
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
                    if (target.active && target.whoAmI != npc.whoAmI && target.Hitbox.Intersects(npc.Hitbox) && target.GetGlobalNPC<InfightingNPC>().infightIframes <= 0)
                    {
                        target.StrikeNPC(infightDamage, infightKnockback, Math.Sign(target.Center.X - npc.Center.X), Main.rand.NextBool(infightCritChance, 100));
                        target.GetGlobalNPC<InfightingNPC>().infightIframes = infightGivenIframes;
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

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (originalNPC != null)
            {
                damage = originalNPC.GetGlobalNPC<InfightingNPC>().infightDamage * 2;
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
