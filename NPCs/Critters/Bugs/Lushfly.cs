using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace Wisplantern.NPCs.Critters.Bugs
{
    class Lushfly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("Cute little firefly-like bugs make their home in the lush caverns.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 10;
            NPC.height = 10;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = -1;
            NPC.noTileCollide = false;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
        }

        int frame = 0;
        int lightCounter = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 4)
            {
                NPC.frameCounter = 0;
                frame++;
                if (frame >= Main.npcFrameCount[Type])
                {
                    frame = 0;
                }
            }

            NPC.frame.Y = frameHeight * frame;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome<Biomes.LushCave>())
            {
                return SpawnCondition.Cavern.Chance * 1f;
            }
            else
            {
                return 0f;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (lightCounter > 0)
            {
                SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(ModContent.Request<Texture2D>("Wisplantern/NPCs/Critters/Bugs/Lushfly_Glow").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0f);
            }
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                bool shouldSync = false;

                NPC.ai[0]--;
                if (NPC.ai[0] <= 0)
                {
                    NPC.ai[0] = Main.rand.Next(60, 240);
                    NPC.ai[1] = Main.rand.NextFloat(-0.1f, 0.1f);
                    NPC.ai[2] = Main.rand.NextFloat(-0.1f, 0.1f);

                    shouldSync = true;
                }

                NPC.velocity.X += NPC.ai[1];
                NPC.velocity.Y += NPC.ai[2];

                if (NPC.collideX)
                {
                    NPC.velocity.X *= -0.5f;
                    NPC.ai[1] *= -1;
                    shouldSync = true;
                }

                if (NPC.collideY)
                {
                    NPC.velocity.Y *= -0.5f;
                    NPC.ai[2] *= -1;
                    shouldSync = true;
                }

                NPC.spriteDirection = NPC.ai[1] > 0 ? 1 : -1;

                NPC.velocity *= 0.97f;

                if (shouldSync)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }

                if (lightCounter > 0)
                {
                    lightCounter--;
                    Lighting.AddLight(NPC.Center, new Vector3(143, 255, 248) / 300f);
                }
                else
                {
                    if (Main.rand.NextBool(300))
                    {
                        lightCounter = Main.rand.Next(25, 60);
                    }
                }
            }
        }
    }
}
