using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using System;
using Terraria.Graphics.CameraModifiers;
using Terraria.Audio;
using Terraria;

namespace Wisplantern.NPCs.Enemies
{
    internal class Moonfly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wisplantern");
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Is it an insect? Is it a bird? Who knows!? It'll kill you though!")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 34;
            NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit45;
            NPC.DeathSound = SoundID.NPCDeath47;
            NPC.aiStyle = -1;
            NPC.noTileCollide = false;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.defense = 4;
            NPC.damage = 15;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 3, 0);
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);

                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.GhostRoseDust>(), velocity.X, velocity.Y);
                }
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter >= 1)
            {
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }

                NPC.frameCounter = 0;
            }

            NPC.frame = new Rectangle(0, frameHeight * frame, 42, frameHeight);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float thingy = 2f;
            float r = 0.49f / thingy;
            float g = 0.78f / thingy;
            float b = 1.00f / thingy;

            Lighting.AddLight(NPC.Center, r, g, b);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.InModBiome<Biomes.LushCave>())
            {
                return 0f;
            }

            return SpawnCondition.Cavern.Chance * 0.2f;
        }

        float speed = 10f;
        public override void AI()
        {
            NPC.TargetClosest(true);

            NPC.spriteDirection = NPC.direction;

            if (NPC.ai[0] <= 0)
            {
                NPC.ai[0] = Main.rand.Next(50, 90);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    bool positionFound = false;
                    Vector2 potentialPosition;
                    int tries = 0;

                    while (!positionFound || tries > 300)
                    {
                        potentialPosition = NPC.position + Main.rand.NextVector2Circular(250f, 125f);
                        tries++;

                        if (Collision.CanHit(NPC.position, NPC.width, NPC.height, potentialPosition, NPC.width, NPC.height) && (potentialPosition.Distance(Main.player[NPC.target].Center) < NPC.Distance(Main.player[NPC.target].Center) || tries > 60))
                        {
                            positionFound = true;

                            NPC.ai[1] = potentialPosition.X;
                            NPC.ai[2] = potentialPosition.Y;
                        }
                    }

                    NPC.netUpdate = true;
                }

            }
            else if (NPC.position.Distance(new Vector2(NPC.ai[1], NPC.ai[2])) <= speed)
            {
                NPC.ai[0]--;
                NPC.position = new Vector2(NPC.ai[1], NPC.ai[2]);
                NPC.velocity = Vector2.Zero;
            }
            else
            {
                NPC.ai[0]--;
                NPC.velocity = (new Vector2(NPC.ai[1], NPC.ai[2]) - NPC.position).ToRotation().ToRotationVector2() * speed;
            }
        }
    }
}
