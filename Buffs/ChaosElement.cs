using System.IO;
using Terraria.Audio;
using Wisplantern.Globals.GNPCs;

namespace Wisplantern.Buffs
{
    class ChaosElement : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public static void Teleport(NPC npc, Vector2 target)
        {
            SoundEngine.PlaySound(SoundID.Item6, npc.Center);

            if (Main.netMode != NetmodeID.Server)
            {
                for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 50f)
                {
                    Vector2 velocity = i.ToRotationVector2() * Main.rand.NextFloat(5f, 10f);
                    Dust dust = Dust.NewDustPerfect(npc.Center, DustID.MagicMirror);
                    dust.velocity = velocity;
                }
            }

            npc.Center = target;

            if (Main.netMode != NetmodeID.Server)
            {
                for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 50f)
                {
                    Vector2 velocity = i.ToRotationVector2() * Main.rand.NextFloat(5f, 10f);
                    Dust dust = Dust.NewDustPerfect(npc.Center, DustID.MagicMirror);
                    dust.velocity = velocity;
                }
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            InfightingNPC iNPC = npc.GetGlobalNPC<InfightingNPC>();

            if (iNPC.infightPlayer == Main.myPlayer)
            {
                if (iNPC.aggravated)
                {
                    iNPC.chaosTeleportCooldown--;

                    if (iNPC.chaosTeleportCooldown <= 0)
                    {
                        NPC t = iNPC.GetNPCTarget(npc);

                        if (t != null)
                        {
                            Vector2 target = t.Center;

                            if (!Collision.SolidCollision(target - npc.Size / 2, npc.width, npc.height))
                            {
                                iNPC.chaosTeleportCooldown = Main.rand.Next(120, 300);
                                Teleport(npc, target);
                                if (Main.netMode != NetmodeID.SinglePlayer) Mod.SendPacket(new SyncChaosTeleport(npc.whoAmI, target.X, target.Y), forward: true);
                            }
                            else
                            {
                                for (int n = 0; n < 30; n++)
                                {
                                    target = t.Center + Main.rand.NextVector2FromRectangle(new Rectangle(-t.width - npc.width, -t.height - npc.height, t.width * 2 + npc.width * 2, t.height * 2 + npc.height * 2)) / 2f;

                                    if (!Collision.SolidCollision(target - npc.Size / 2, npc.width, npc.height))
                                    {
                                        iNPC.chaosTeleportCooldown = Main.rand.Next(120, 300);
                                        Teleport(npc, target);
                                        if (Main.netMode != NetmodeID.SinglePlayer) Mod.SendPacket(new SyncChaosTeleport(npc.whoAmI, target.X, target.Y), forward: true);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public readonly struct SyncChaosTeleport : IEasyPacket<SyncChaosTeleport>
        {
            public readonly int npc;
            public readonly float x;
            public readonly float y;

            public SyncChaosTeleport(int npc, float x, float y)
            {
                this.npc = npc;
                this.x = x;
                this.y = y;
            }

            public SyncChaosTeleport Deserialise(BinaryReader reader, in SenderInfo sender)
            {
                return new SyncChaosTeleport(reader.ReadInt32(), reader.ReadSingle(), reader.ReadSingle());
            }

            public void Serialise(BinaryWriter writer)
            {
                writer.Write(npc);
                writer.Write(x);
                writer.Write(y);
            }
        }

        public readonly struct SyncChaosTeleportHandler : IEasyPacketHandler<SyncChaosTeleport>
        {
            void IEasyPacketHandler<SyncChaosTeleport>.Receive(in SyncChaosTeleport packet, in SenderInfo sender, ref bool handled)
            {
                Teleport(Main.npc[packet.npc], new Vector2(packet.x, packet.y));
                handled = true;
            }
        }
    }
}