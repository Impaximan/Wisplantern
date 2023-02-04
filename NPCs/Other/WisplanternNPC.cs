using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using System;
using Terraria.Graphics.CameraModifiers;
using Terraria.Audio;

namespace Wisplantern.NPCs.Other
{
    class WisplanternNPC : ModNPC
    {
        public override bool CheckActive()
        {
            return NPC.ai[3] <= 0;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wisplantern");
            Main.npcFrameCount[Type] = 9;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Mysterious lanterns said to contain spirits linger in the caverns, waiting to be uncovered. Fleeing on contact, the Wisplanterns will eventually shatter if damaged enough.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 54;
            NPC.height = 56;
            NPC.lifeMax = 3;
            SoundStyle hitStyle = SoundID.DD2_WitherBeastCrystalImpact;
            hitStyle.Volume *= 1.5f;
            NPC.HitSound = SoundID.DD2_WitherBeastCrystalImpact;
            SoundStyle deathStyle = SoundID.DD2_WitherBeastDeath;
            deathStyle.Volume *= 2f;
            NPC.DeathSound = deathStyle;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.defense = 9999;
            NPC.rarity = 2;
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<WisplanternVortex>(), 0, 0f);
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.ai[1] = NPC.Center.X;
            NPC.ai[2] = NPC.Center.Y;
            frame = 0;

            Vector2 position = Vector2.Lerp(new Vector2(NPC.ai[1], NPC.ai[2]).FindGroundUnder(), new Vector2(NPC.ai[1], NPC.ai[2]).FindCeilingAbove(), 0.5f);
            NPC.ai[1] = position.X;
            NPC.ai[2] = position.Y;
        }

        int timeUntilRainbowGun = 0;
        public override void HitEffect(int hitDirection, double damage)
        {
            if (!protecting)
            {
                NPC.DiscourageDespawn(1200);
                NPC.ai[0] += 1;

                if (Main.player[Main.myPlayer].Distance(NPC.Center) <= 3000)
                {
                    Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<Buffs.Hyperspeed>(), 900);
                    Wisplantern.freezeFrames = 10;
                    Wisplantern.freezeFrameLight = true;

                    PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, Main.rand.NextVector2CircularEdge(1f, 1f), Main.rand.NextFloat(15f, 25f), 6f, 20, 1000f);
                    Main.instance.CameraModifiers.Add(modifier);

                    for (int i = 0; i < Main.rand.Next(125, 150); i++)
                    {
                        Dust dust = Main.dust[Dust.NewDust(NPC.Center, 1, 1, ModContent.DustType<Dusts.HyperstoneDust>())];
                        dust.velocity = Main.rand.NextVector2Circular(10f, 10f);
                    }

                    timeUntilRainbowGun = 2;
                }

                NPC.ai[3] = 3000;
                FindNewPosition();

                moveSpeed = 10f;
            }
            NPC.life = NPC.lifeMax - (int)NPC.ai[0];


            NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);

        }

        public void FindNewPosition()
        {
            NPC.ai[2] += 500;
            NPC.ai[1] += Main.rand.Next(-750, 750);

            int amount = 0;
            while (Main.tileSolid[Main.tile[(int)NPC.ai[1] / 16, (int)NPC.ai[2] / 16].TileType])
            {
                NPC.ai[2] += 2f;
                amount++;
                if (amount > 750)
                {
                    break;
                }
            }

            NPC.ai[2] = new Vector2(NPC.ai[1], NPC.ai[2]).FindGroundUnder().Y - 10f;
            Vector2 position = Vector2.Lerp(new Vector2(NPC.ai[1], NPC.ai[2]).FindGroundUnder(), new Vector2(NPC.ai[1], NPC.ai[2]).FindCeilingAbove(), 0.5f);
            NPC.ai[1] = position.X;
            NPC.ai[2] = position.Y;

        }

        int frame = 0;
        bool protecting = false;
        public override void FindFrame(int frameHeight)
        {
            int extraFrames = 0;
            if (protecting) extraFrames += 4;

            if (NPC.ai[0] == 0)
            {
                frame = 0;
                extraFrames = 0;
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 4)
                {
                    NPC.frameCounter = 0;
                    frame++;
                    if (frame > 4)
                    {
                        frame = 1;
                    }
                }
            }

            NPC.frame.Y = frameHeight * (frame + extraFrames);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Cavern.Chance * 0.075f;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(ModContent.Request<Texture2D>("Wisplantern/NPCs/Other/WisplanternNPC_Glow").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0f);

            if (NPC.ai[0] > 0)
            {
                Lighting.AddLight(NPC.Center, 187 / 300f, 206 / 300f, 238 / 300f);
            }
            else
            {
                Lighting.AddLight(NPC.Center, 187 / 600f, 206 / 600f, 238 / 600f);
            }
        }

        float timeAlive = 0f;
        float moveSpeed = 5f;
        public override void AI()
        {
            timeAlive++;
            Vector2 targetPosition =  new Vector2(NPC.ai[1], NPC.ai[2]) + new Vector2(0f, (float)Math.Sin(timeAlive / 50f) * 10f);
            
            NPC.velocity = (targetPosition - NPC.Center) / 50;
            moveSpeed = MathHelper.Lerp(moveSpeed, 1.5f, 0.08f);
            if (NPC.velocity.Length() > moveSpeed) NPC.velocity = Vector2.Normalize(NPC.velocity) * moveSpeed;
            protecting = NPC.Distance(targetPosition) >= 50;
            NPC.dontTakeDamage = protecting;

            if (NPC.ai[3] > 0)
            {
                NPC.ai[3]--;
            }

            if (timeUntilRainbowGun > 0)
            {
                timeUntilRainbowGun--;
                if (timeUntilRainbowGun <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item67, NPC.Center);
                }
            }
        }
    }

    class WisplanternVortex : ModProjectile
    {
        public override string Texture => "Wisplantern/VFX/Vortex4";

        int trueTimeLeft = 180;
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = trueTimeLeft + 60;
            Projectile.width = 1000;
            Projectile.height = 1000;
            Projectile.tileCollide = false;
            Projectile.scale = 0.3f;
            Projectile.alpha = 255;
        }

        public void SpawnItem()
        {
            Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Center, (int)Projectile.ai[0]);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Wisplantern/VFX/Vortex4").Value, //Texture
                Projectile.Center - Main.screenPosition, //Position
                null, //Source Rectangle
                Projectile.GetAlpha(new Color(187, 206, 238)) * 0.5f, //Color
                Projectile.rotation, //Rotation
                new Vector2(500, 500), //Origin
                Projectile.scale, //Scale
                SpriteEffects.None, //SpriteEffects
                0f); //LayerDepth
            return false;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5f);

            if (trueTimeLeft > 0)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 255 / 60;
                }
                trueTimeLeft--;
                if (trueTimeLeft == 90 && Wisplantern.wisplanternLoot.Count > 0)
                {
                    Projectile.ai[0] = Main.rand.Next(Wisplantern.wisplanternLoot);
                    NetMessage.SendData(MessageID.SyncProjectile, number: Projectile.whoAmI);
                }
                if (trueTimeLeft == 60 && Wisplantern.wisplanternLoot.Count > 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient) SpawnItem();
                }
            }
            else
            {
                Projectile.scale *= 0.98f;
                Projectile.alpha += 255 / 60;
            }
        }
    }
}
