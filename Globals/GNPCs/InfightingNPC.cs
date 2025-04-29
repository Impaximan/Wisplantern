using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework.Graphics;
using Wisplantern.Buffs;
using Terraria.ModLoader.IO;
using System.IO;

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

        public float attackSpeedMult = 1f;
        public float attackSpeedCounter = 0f;

        public int chaosTeleportCooldown = 120;

        public override void ResetEffects(NPC npc)
        {
            attackSpeedMult = 1f;
        }

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

        public static List<int> infightingBlacklist = new()
        {
            NPCID.EaterofWorldsBody,
            NPCID.EaterofWorldsTail
        };

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(aggravation);
            binaryWriter.Write(aggravated);
            if (aggravated)
            {
                binaryWriter.Write(infightDamage);
                binaryWriter.Write(infightKnockback);
                binaryWriter.Write(infightCritChance);
                binaryWriter.Write(infightPlayer);
            }
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            aggravation = binaryReader.ReadSingle();
            aggravated = binaryReader.ReadBoolean();
            if (aggravated)
            {
                infightDamage = binaryReader.ReadInt32();
                infightKnockback = binaryReader.ReadSingle();
                infightCritChance = binaryReader.ReadInt32();
                infightPlayer = binaryReader.ReadInt32();
            }
        }

        public List<int> eaterOfWorldsSegments = new();

        public NPC GetNPCTarget(NPC me)
        {
            NPCAimedTarget data = me.GetTargetData();
            if (data.Type != Terraria.Enums.NPCTargetType.Player)
            {
                switch (data.Type)
                {
                    case Terraria.Enums.NPCTargetType.NPC:
                         return Main.npc[me.target - 300];
                    default:
                        return Main.npc[0];
                }
            }

            NPC target = null;

            float distance = 500f;
            float decoyDistance = 500f;

            if (me.target < 256)
            {
                distance = Main.player[me.target].Distance(me.Center) * 1.5f;
                decoyDistance = Main.player[me.target].Distance(me.Center) * 1.5f;
            }
            bool foundDecoy = false;
            bool foundBoss = false;

            foreach (NPC npc in Main.npc)
            {
                bool prioritize = (npc.Distance(me.Center) < distance || Main.player[me.target].GetModPlayer<ModPlayers.ManipulativePlayer>().smokeBombTime > 0 || npc.boss) && !foundDecoy && !foundBoss;
                if (npc.TryGetGlobalNPC(out InfightingNPC result))
                {
                    if (result.decoy && (npc.Distance(me.Center) < decoyDistance || Main.player[me.target].GetModPlayer<ModPlayers.ManipulativePlayer>().smokeBombTime > 0)) prioritize = true;
                }
                if (npc.active && prioritize && npc.whoAmI != me.whoAmI && !npc.dontTakeDamage && !(me.aiStyle == 9 && npc.whoAmI == castAIOwner) && npc.aiStyle != 9 && npc.whoAmI != me.realLife && npc.realLife != me.whoAmI && (npc.realLife != me.realLife || npc.realLife == -1) &&
                        !(me.type == NPCID.EaterofWorldsHead && eaterOfWorldsSegments.Contains(npc.whoAmI)) && !NPCID.Sets.ProjectileNPC[npc.type])
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
            NPCAimedTarget data = me.GetTargetData();
            if (data.Type != Terraria.Enums.NPCTargetType.Player)
            {
                return false;
            }

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
                    NPCAimedTarget data = npc.GetTargetData();
                    if (data.Type != Terraria.Enums.NPCTargetType.Player)
                    {
                        return base.PreAI(npc);
                    }
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

        public static int GetAggravationTime(NPC NPC)
        {
            if (NPC.SpawnedFromStatue) return 1200;
            return 600;
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (npc.HasBuff<Vulnerable>())
            {
                return Color.Lerp(drawColor, Color.LightBlue * (1f - npc.alpha / 255f), 0.5f);
            }

            return base.GetAlpha(npc, drawColor);
        }

        public override void OnKill(NPC npc)
        {
            if (targetNPC != null && originalPlayerPosition.HasValue && shouldTeleportBack)
            {
                Main.player[ogTarget].position = originalPlayerPosition.Value;
                shouldTeleportBack = false;
            }

            eaterOfWorldsSegments.Clear();

            ManipulativePlayer mPlayer = Main.player[infightPlayer].GetModPlayer<ManipulativePlayer>();
            if (aggravated && !NPCID.Sets.ProjectileNPC[npc.type] && Main.rand.NextBool() && mPlayer.charisma < mPlayer.MaxCharisma)
            {
                Item.NewItem(new EntitySource_Loot(npc), npc.getRect(), ModContent.ItemType<Items.Powerups.CharismaPickup>());
            }
        }

        public override void PostAI(NPC npc)
        {
            if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
            {
                int currentSegment = npc.whoAmI;

                int tries = 0;

                while (Main.npc[currentSegment] == null || !Main.npc[currentSegment].active || Main.npc[currentSegment].type != NPCID.EaterofWorldsHead)
                {
                    currentSegment = (int)Main.npc[currentSegment].ai[1];
                    tries++;

                    if (tries > 300 || currentSegment >= Main.npc.Length)
                    {
                        break;
                    }
                }

                if (currentSegment <= Main.npc.Length && Main.npc[currentSegment] != null && Main.npc[currentSegment].active && Main.npc[currentSegment].type == NPCID.EaterofWorldsHead)
                {
                    InfightingNPC iNPC = Main.npc[currentSegment].GetGlobalNPC<InfightingNPC>();
                    
                    if (!iNPC.eaterOfWorldsSegments.Contains(npc.whoAmI))
                    {
                        iNPC.eaterOfWorldsSegments.Add(npc.whoAmI);
                    }
                }
            }


            if (targetNPC != null && originalPlayerPosition.HasValue && shouldTeleportBack)
            {
                Main.player[ogTarget].position = originalPlayerPosition.Value;
                shouldTeleportBack = false;
            }

            if (aggravated)
            {
                if (!Main.player[infightPlayer].dead)
                {
                    npc.DiscourageDespawn(300);
                }

                foreach (NPC target in Main.npc)
                {
                    if (target.active && target.whoAmI != npc.whoAmI && target.Hitbox.Intersects(npc.Hitbox) && target.GetGlobalNPC<InfightingNPC>().infightIframes <= 0 && !target.friendly && !target.dontTakeDamage && 
                        target.realLife != npc.whoAmI && target.whoAmI != npc.realLife && (target.realLife != npc.realLife || target.realLife == -1) &&
                        !(npc.type == NPCID.EaterofWorldsHead && target.ai[1] == npc.whoAmI))
                    {
                        int damage = infightDamage;
                        if (npc.SpawnedFromStatue)
                        {
                            damage = (int)(damage * 1.35f);
                        }

                        if (infightPlayer == Main.myPlayer)
                        {
                            NPC.HitInfo info = target.CalculateHitInfo(damage, Math.Sign(target.Center.X - npc.Center.X), Main.rand.NextBool(infightCritChance, 100), infightKnockback, ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>(), true, Main.player[infightPlayer].luck);
                            int struckDamage = target.StrikeNPC(info);
                            if (Main.netMode != NetmodeID.SinglePlayer) NetMessage.SendStrikeNPC(target, info, Main.myPlayer);
                            Main.player[infightPlayer].addDPS(struckDamage);
                            //foreach (int buffType in npc.buffType)
                            //{
                            //    target.AddBuff(buffType, npc.buffTime[npc.FindBuffIndex(buffType)]);
                            //}

                            if (infightItem != null && infightItem.ModItem != null)
                            {
                                infightItem.ModItem.OnHitNPC(Main.player[infightPlayer], target, info, struckDamage);

                                foreach (GlobalItem gItem in infightItem.Globals)
                                {
                                    gItem.OnHitNPC(infightItem, Main.player[infightPlayer], target, info, struckDamage);
                                }
                            }

                            target.GetGlobalNPC<InfightingNPC>().infightIframes = infightGivenIframes;
                            foreach (GlobalNPC gNPC in target.Globals)
                            {
                                gNPC.OnHitByItem(target, Main.player[infightPlayer], infightItem, info, damage);
                            }
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

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff<Vulnerable>() && !modifiers.DamageType.CountsAsClass(ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>()))
            {
                modifiers.SourceDamage *= 1.35f;
            }
        }

        float vulnerableArrowRotation = 0f;
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.HasBuff<Buffs.Vulnerable>())
            {
                vulnerableArrowRotation += MathHelper.Pi / 90f;

                for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.PiOver2)
                {
                    float direction = i + vulnerableArrowRotation;
                    Texture2D texture = ModContent.Request<Texture2D>("Wisplantern/Items/Weapons/Manipulative/Canes/GuidingLightArrow").Value;
                    spriteBatch.Draw(texture, npc.Center + direction.ToRotationVector2() * (npc.Size.Length() / 2f + 15f) - Main.screenPosition, null, Color.White * 0.5f, direction, texture.Size() / 2f, 1f + (float)Math.Sin(vulnerableArrowRotation * 3f) * 0.2f, SpriteEffects.None, 0f);
                }
            }
        }
    }

    class InfightProjectile : GlobalProjectile
    {
        NPC originalNPC;
        Item infightItem = null;
        int infightPlayer = 0;

        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent)
            {
                EntitySource_Parent parentSource = source as EntitySource_Parent;
                if (parentSource.Entity is NPC)
                {
                    originalNPC = parentSource.Entity as NPC;
                    InfightingNPC infightingNPC = originalNPC.GetGlobalNPC<InfightingNPC>();

                    if (infightingNPC.aggravated)
                    {
                        projectile.friendly = true;

                        if (infightingNPC.infightItem != null)
                        {
                            infightItem = infightingNPC.infightItem;
                        }

                        infightPlayer = infightingNPC.infightPlayer;
                    }
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[infightPlayer] != null && infightItem != null)
            {
                if (infightItem != null && infightItem.ModItem != null)
                {
                    infightItem.ModItem.OnHitNPC(Main.player[infightPlayer], target, hit, damageDone);
                }

                foreach (GlobalNPC gNPC in target.Globals)
                {
                    gNPC.OnHitByItem(target, Main.player[infightPlayer], infightItem, hit, damageDone);
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
