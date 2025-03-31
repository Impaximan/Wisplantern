using Terraria.Audio;
using Terraria.DataStructures;
using Terraria;
using Wisplantern.Items.Weapons.Manipulative.Canes;

namespace Wisplantern.Items.Weapons.Manipulative.Decoys
{
    class AlluringAmethyst : DecoyItem
    {
        public override int DecoyType => ModContent.NPCType<AlluringAmethystDecoy>();
        public override int CooldownSeconds => 35;
        public override string DecoyName => "alluring amethyst";
        public override int DefaultHP => 50;

        public override void SetStats()
        {
            Item.damage = 8;
            Item.width = 18;
            Item.height = 28;
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.consumable = false;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 35, 0);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 3f;
            Item.rare = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 2)
                .AddIngredient(ItemID.Amethyst, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class AlluringAmethystDecoy : DecoyNPC
    {
        public override bool CanBeAttackedByOtherPlayers => true;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
        }

        public override string Texture => "Wisplantern/Items/Weapons/Manipulative/Decoys/AlluringAmethyst";

        public override void SetStats()
        {
            NPC.width = 18;
            NPC.height = 28;
            NPC.DeathSound = SoundID.DD2_CrystalCartImpact;
            NPC.HitSound = SoundID.DD2_WitherBeastCrystalImpact;
            NPC.noGravity = true;
            NPC.defense = 10;
            NPC.noTileCollide = false;
        }

        public override void AI()
        {
            NPC.rotation = NPC.velocity.Length() * -Math.Sign(NPC.velocity.X);
            NPC.velocity *= 0.98f;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override void OnKill()
        {
            if (NPC.ai[2] == 1f)
            {
                return;
            }

            for (int n = 0; n < Main.rand.Next(3, 5); n++)
            {
                Projectile.NewProjectile(new EntitySource_OnHurt(NPC, null),
                    NPC.Center,
                    Main.rand.NextFloat((float)Math.PI * 2f).ToRotationVector2() * Main.rand.NextFloat(5f, 10f),
                    ModContent.ProjectileType<AmethystSoul>(),
                    (int)(NPC.damage * 2f),
                    0f,
                    (int)NPC.ai[0]);
            }

            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, NPC.Center);
        }
    }

    class AmethystSoul : ModProjectile
    {
        public override string Texture => "Wisplantern/Items/Weapons/Manipulative/Decoys/AlluringAmethyst";

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (timeAlive > timeUntilHoming)
            {
                return null;
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, target.Center);
        }

        const int timeUntilHoming = 20;
        int timeAlive = 0;
        float angleLerpAmount = 0f;

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, Projectile.velocity.X, Projectile.velocity.Y);

            timeAlive++;

            if (timeAlive >= timeUntilHoming)
            {
                angleLerpAmount += 0.1f / 60f;

                if (timeAlive > timeUntilHoming * 2f)
                {
                    angleLerpAmount += 1f / 30f;
                }

                if (angleLerpAmount > 1f)
                {
                    angleLerpAmount = 1f;
                }

                float distance = 1000f;

                NPC target = null;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    float dist = npc.Distance(Projectile.Center);

                    if (npc.active && !npc.friendly && dist < distance)
                    {
                        target = npc;
                        distance = dist;
                    }
                }

                if (target != null)
                {
                    Projectile.alpha = 0;

                    Projectile.velocity = Projectile.velocity.ToRotation().AngleLerp(Projectile.DirectionTo(target.Center).ToRotation(), angleLerpAmount).ToRotationVector2() * (Projectile.velocity.Length() + 0.1f);
                }
                else
                {
                    Projectile.velocity *= 0.93f;
                    Projectile.alpha += 5;

                    if (Projectile.alpha > 255)
                    {
                        Projectile.active = false;
                    }
                }
            }
        }
    }
}
