using Terraria.DataStructures;
using Terraria.Audio;
using System;

namespace Wisplantern.Items.Tools.Movement
{
    class Hooklantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Can be thrown and grappled to" +
                "\nOnly two can be active at once"); */
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.noUseGraphic = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<HooklanternProjectile>();
            Item.shootSpeed = 15f;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.mana = 20;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.SetScholarlyDescription("Found mysteriously from the mystical remains of Wisplanterns underground");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool CanUseItem(Player player)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.owner == player.whoAmI && projectile.active && projectile.type == Item.shoot)
                {
                    count++;
                    if (count >= 2)
                    {
                        return false;
                    }
                }
            }
            return base.CanUseItem(player);
        }
    }

    class HooklanternProjectile : ModProjectile
    {
        public override string Texture => "Wisplantern/Items/Tools/Movement/Hooklantern";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hooklantern");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 600;
            Projectile.width = 20;
            Projectile.height = 34;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = oldVelocity.X * -1;
            }
            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                Projectile.velocity.Y = oldVelocity.Y * -1;
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 20;
            height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = -10f * Math.Sign(Projectile.velocity.X);
        }

        public override void AI()
        {
            Projectile.rotation *= 0.9f;
            Projectile.velocity *= 0.95f;

            Projectile.ai[0]++;
            if (Projectile.ai[0] == 30)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < Main.rand.Next(10, 15); i++)
                    {
                        Dust dust = Main.dust[Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<Dusts.HyperstoneDust>())];
                        dust.velocity = Main.rand.NextVector2Circular(1f, 1f);
                    }
                }

                SoundStyle style = SoundID.Item9;
                style.MaxInstances = 0;
                SoundEngine.PlaySound(style, Projectile.Center);
            }

            Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            if (player.Distance(Projectile.Center) <= 30f && Projectile.ai[0] > 30 && Projectile.ai[1] != 0)
            {
                player.velocity *= 1.2f;
                Projectile.timeLeft = 0;
                NetMessage.SendData(MessageID.SyncProjectile, number: Projectile.whoAmI);
            }

            Projectile.ai[1] = 0;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.netUpdate = true;
            NetMessage.SendData(MessageID.KillProjectile, number: Projectile.whoAmI);
            Wisplantern.freezeFrames = 5;
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, Projectile.Center);
            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < Main.rand.Next(25, 35); i++)
                {
                    Dust dust = Main.dust[Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<Dusts.HyperstoneDust>())];
                    dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
                }
            }
        }
    }

    class HooklanternGlobal : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        bool hasGrappled = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            hasGrappled = false;

            if (Main.projHook[projectile.type])
            {
                float hookDistance = 300f;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile hooklantern = Main.projectile[i];
                    if (hooklantern != null && hooklantern.type == ModContent.ProjectileType<HooklanternProjectile>() && hooklantern.active && hooklantern.Distance(Main.MouseWorld) < hookDistance)
                    {
                        hookDistance = hooklantern.Distance(projectile.Center);
                        if (projectile.owner == Main.myPlayer)
                        {
                            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(hooklantern.Center + hooklantern.Distance(projectile.Center) / projectile.velocity.Length() * hooklantern.velocity * 0.95f).ToRotation(), MathHelper.ToRadians(15f)).ToRotationVector2() * projectile.velocity.Length();
                            NetMessage.SendData(MessageID.SyncProjectile, number: projectile.whoAmI);
                        }
                    }
                }
            }
        }

        public override void PostAI(Projectile projectile)
        {
            if (Main.projHook[projectile.type])
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile hooklantern = Main.projectile[i];
                    if (hooklantern != null && hooklantern.type == ModContent.ProjectileType<HooklanternProjectile>() && hooklantern.active && hooklantern.Distance(projectile.Center) <= 30f && hooklantern.ai[0] > 30)
                    {
                        if (!hasGrappled)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit42, hooklantern.position);
                        }
                        SetGrapple(hooklantern.Center, projectile);
                        hooklantern.ai[1] = 1;
                    }
                }
            }
        }

        public void SetGrapple(Vector2 position, Projectile projectile)
        {
            hasGrappled = true;
            projectile.ai[0] = 2;
            projectile.position = position;
            projectile.position -= projectile.Size / 2;
            Main.player[projectile.owner].grappling[Main.player[projectile.owner].grapCount] = projectile.whoAmI;
            Main.player[projectile.owner].grapCount++;
            projectile.velocity = Vector2.Zero;
            projectile.netUpdate = true;
        }
    }
}
