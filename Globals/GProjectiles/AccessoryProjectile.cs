using System.IO;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Wisplantern.Items.Equipable.Accessories;

namespace Wisplantern.Globals.GProjectiles
{
    public class AccessoryProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool frontlinerSummon = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            frontlinerSummon = false;

            if (source is EntitySource_ItemUse itemSource && projectile.minionSlots > 0)
            {
                bool b = true;
                foreach (Projectile minion in Main.projectile)
                {
                    if (minion.minionSlots > 0 && minion.owner == projectile.owner && minion.active && minion != projectile && minion.GetGlobalProjectile<AccessoryProjectile>().frontlinerSummon)
                    {
                        b = false;
                        break;
                    }
                }

                if (b)
                {
                    if (itemSource.Player.AccessoryActive<FrontlinersSpearhead>())
                    {
                        CombatText.NewText(projectile.getRect(), Color.Beige, "Spearhead Equipped!", true);

                        SoundStyle sound = new("Wisplantern/Sounds/Effects/DrawSword");
                        sound.Volume *= 0.2f;
                        sound.PitchVariance = 0.15f;
                        SoundEngine.PlaySound(sound, projectile.Center);
                    }

                    frontlinerSummon = true;
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[projectile.owner].AccessoryActive<DraconicDacron>())
            {
                if (projectile.type == ProjectileID.FireArrow)
                {
                    target.AddBuff(BuffID.OnFire, 180);
                }
                if (projectile.type == ProjectileID.FrostburnArrow)
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }
        }


        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (frontlinerSummon)
            {
                binaryWriter.Write(spearheadAttackCooldown);
                binaryWriter.Write(spearheadThrowCooldown);
            }
        }

        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            if (frontlinerSummon)
            {
                spearheadAttackCooldown = binaryReader.ReadInt32();
                spearheadThrowCooldown = binaryReader.ReadInt32();
            }
        }

        public override void AI(Projectile projectile)
        {
            UpdateFrontlinersSpearhead(projectile);
        }

        public int spearheadAttackCooldown = 0;
        public int spearheadThrowCooldown = 0;
        public void UpdateFrontlinersSpearhead(Projectile projectile)
        {
            if (spearheadAttackCooldown > 0)
            {
                spearheadAttackCooldown--;
            }

            if (spearheadThrowCooldown > 0)
            {
                spearheadThrowCooldown--;
            }

            if (frontlinerSummon && Main.player[projectile.owner].AccessoryActive<FrontlinersSpearhead>())
            {
                NPC target = null;

                if (Main.player[projectile.owner].HasMinionAttackTargetNPC)
                {
                    target = projectile.OwnerMinionAttackTargetNPC;
                }
                else
                {
                    int num = -1;
                    projectile.Minion_FindTargetInRange(500, ref num, false);
                    if (num != -1) target = Main.npc[num];
                }

                if (target != null && spearheadAttackCooldown <= 0)
                {
                    if (target.Distance(projectile.Center) < 150f)
                    {
                        if (Main.netMode != NetmodeID.Server) SoundEngine.PlaySound(SoundID.Item1, projectile.Center);

                        if (Main.myPlayer == projectile.owner)
                        {
                            int p = Projectile.NewProjectile(new EntitySource_Parent(projectile, "FrontlinersSpearheadAttack"),
                                projectile.Center,
                                projectile.DirectionTo(target.Center),
                                ModContent.ProjectileType<FrontlinersSpearheadSpear>(),
                                projectile.damage,
                                0.5f,
                                projectile.owner,
                                projectile.whoAmI);


                            Main.projectile[p].netUpdate = true;
                            spearheadAttackCooldown = FrontlinersSpearheadSpear.totalTime + 10;
                        }
                    }
                    else if (spearheadThrowCooldown <= 0)
                    {
                        if (Main.netMode != NetmodeID.Server) SoundEngine.PlaySound(SoundID.Item1, projectile.Center);

                        if (Main.myPlayer == projectile.owner)
                        {
                            int p = Projectile.NewProjectile(new EntitySource_Parent(projectile, "FrontlinersSpearheadAttack"),
                                projectile.Center,
                                projectile.DirectionTo(target.Center) * 25f,
                                ModContent.ProjectileType<FrontlinersSpearheadJavelin>(),
                                projectile.damage,
                                4f,
                                projectile.owner,
                                projectile.whoAmI);


                            Main.projectile[p].netUpdate = true;
                            spearheadAttackCooldown = 20;
                            spearheadThrowCooldown = 120;
                        }
                    }
                }
            }
        }
    }
}
