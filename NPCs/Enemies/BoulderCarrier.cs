using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace Wisplantern.NPCs.Enemies
{
    internal class BoulderCarrier : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wisplantern");
            Main.npcFrameCount[Type] = 17;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Dr. Bones comes full circle! Who could've predicted that?")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.lifeMax = 60;
            if (Main.expertMode)
            {
                if (Main.hardMode)
                {
                    NPC.lifeMax = 66;

                    if (NPC.downedPlantBoss)
                    {
                        NPC.lifeMax = 264 / 2;
                    }
                }
            }
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            NPC.noTileCollide = false;
            NPC.noGravity = false;
            NPC.lavaImmune = false;
            NPC.defense = 25;
            NPC.damage = 50;
            NPC.knockBackResist = 0.1f;
            NPC.value = Item.buyPrice(0, 0, 1, 0);

        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {

            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;

            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(20f, 31f), NPC.scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false;
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y == 0 && NPC.velocity.X != 0)
            {
                NPC.frameCounter++;

                if (NPC.frameCounter > 4)
                {
                    frame++;
                    NPC.frameCounter = 0;

                    if (frame >= 14)
                    {
                        frame = 0;
                    }
                }

                NPC.frame.Y = frame * frameHeight;
            }
            else
            {
                NPC.frame.Y = 14 * frameHeight;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Cavern.Chance * 0.04f;
        }

        public override void AI()
        {
            base.AI();

            if (NPC.life <= NPC.lifeMax / 2 || (NPC.Distance(Main.player[NPC.target].Center) < 100f && Collision.CanHit(NPC, Main.player[NPC.target])))
            {
                BecomeNormalSkeleton();
            }
        }

        public void BecomeNormalSkeleton()
        {
            Main.BestiaryTracker.Kills.RegisterKill(NPC);

            int p = Projectile.NewProjectile(new EntitySource_Parent(NPC), NPC.position + new Vector2(10f, -5f), NPC.velocity - new Vector2(NPC.direction * -7f, 2f), ProjectileID.Boulder, 50, 5f);

            Main.projectile[p].netUpdate = true;

            if (NPC.life > 0)
            {
                int life = NPC.life;
                NPC.SetDefaults(NPCID.Skeleton);
                NPC.position.X += 10f;
                NPC.life = life;
            }

            NPC.netUpdate = true;
        }
    }
}
