using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Wisplantern.DamageClasses;
using static System.Net.Mime.MediaTypeNames;

namespace Wisplantern.Items.Weapons.Manipulative.Misc
{
    public class FumeShroom : ModItem
    {
        public override bool WeaponPrefix()
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.SetManipulativePower(0.11f);
            Item.DamageType = ModContent.GetInstance<ManipulativeDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<ShroomFume>();
            Item.width = 42;
            Item.height = 42;
            Item.useTime = 3;
            Item.useAnimation = 24;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Blue;
            SoundStyle fart = new SoundStyle("Wisplantern/Sounds/Effects/FartSqueeze");
            fart.MaxInstances = 2;
            fart.SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest;
            fart.PitchVariance = 0.5f;
            fart.Volume = 0.3f;
            Item.UseSound = fart;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.SetScholarlyDescription("Found within mushroom chests");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += velocity.ToRotation().ToRotationVector2() * (Item.Size.Length() - 5f);
            while (Collision.SolidCollision(position - new Vector2(14, 14), 28, 28) && position.Distance(player.Center) > 5f)
            {
                position -= velocity.ToRotation().ToRotationVector2();
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile p = Main.projectile[Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.Pi / 9f) * Main.rand.NextFloat(0.2f, 1.1f), type, damage, knockback, player.whoAmI)];
            if (p.ModProjectile is ShroomFume fume)
            {
                fume.playerItem = Item;
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_OnHit(player, target), target.Center, Main.rand.NextVector2Circular(5f, 5f), Item.shoot, player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI)];
                p.timeLeft = 240 + Main.rand.Next(30);

                if (p.ModProjectile is ShroomFume fume)
                {
                    fume.playerItem = Item;
                }
            }
        }
    }

    public class ShroomFume : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.DamageType = ModContent.GetInstance<ManipulativeDamageClass>();
            Projectile.timeLeft = 45 + Main.rand.Next(30);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.ignoreWater = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1f - Projectile.alpha / 255f);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetMaxDamage(1);
        }

        public Item playerItem;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.Hitbox.Intersects(Projectile.Hitbox))
                    {
                        if (playerItem != null)
                        {
                            if (Projectile.IsNPCIndexImmuneToProjectileType(Type, npc.whoAmI))
                            {
                                if (npc.Aggravate(playerItem, Main.player[Projectile.owner]))
                                {
                                    for (int i = 0; i < Main.rand.Next(10, 20); i++)
                                    {
                                        Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_OnHit(Projectile, npc), npc.Center, Main.rand.NextVector2Circular(15f, 15f), Type, Projectile.damage, Projectile.knockBack, Projectile.owner)];
                                        p.timeLeft = 240 + Main.rand.Next(30);

                                        if (p.ModProjectile is ShroomFume fume)
                                        {
                                            fume.playerItem = playerItem;
                                        }
                                    }
                                    SoundEngine.PlaySound(SoundID.Item16, npc.Center);
                                }
                                npc.immune[Projectile.owner] = 0;
                                Projectile.perIDStaticNPCImmunity[Type][npc.whoAmI] = Main.GameUpdateCount + (uint)Projectile.idStaticNPCHitCooldown;
                            }
                        }
                    }
                }
            }

            Projectile.velocity *= 0.95f;

            Projectile.frameCounter++;

            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                if (Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }

                Projectile.frameCounter = 0;
            }

            if (Projectile.timeLeft < 165 / 5)
            {
                Projectile.alpha += 5;
            }
            else
            {
                if (Projectile.alpha > 90)
                {
                    Projectile.alpha -= 15;
                }
            }
        }
    }
}
