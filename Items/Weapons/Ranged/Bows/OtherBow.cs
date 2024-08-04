using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Ranged.Bows
{
    class OtherBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Fires in reverse and relative to you" +
                "\n'Weirdly strange, oddly'"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;
            Item.width = 18;
            Item.height = 44;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 0f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.SetScholarlyDescription("Found mysteriously from the mystical remains of Wisplanterns underground");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 targetPosition = Main.MouseWorld;
            while (!Collision.SolidCollision(position, 1, 1, false) && position.Distance(targetPosition) >= 15 && position.Distance(player.Center) <= 600f)
            {
                position += position.DirectionTo(targetPosition) * 10f;
            }
            velocity.Normalize();
            type = ModContent.ProjectileType<OtherArrow>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[p].netUpdate = true;
                NetMessage.SendData(MessageID.SyncProjectile, number: p);
            }

            return false;
        }
    }

    class OtherArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        Vector2 directionFromPlayer;
        float distanceFromPlayer;
        float returnSpeed = 6f;
        public override void OnSpawn(IEntitySource source)
        {
            directionFromPlayer = Main.player[Projectile.owner].DirectionTo(Projectile.Center);
            distanceFromPlayer = Main.player[Projectile.owner].Distance(Projectile.Center);

            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < Main.rand.Next(15, 22); i++)
                {
                    Dust dust = Main.dust[Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<Dusts.HyperstoneDust>())];
                    dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    dust.scale *= 0.75f;
                }

                //SoundStyle style = SoundID.Item9;
                //style.MaxInstances = 0;
                //SoundEngine.PlaySound(style, Projectile.Center);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.damage > 5)
            {
                Projectile.damage--;
            }
            returnSpeed += 1f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.DirectionTo(Main.player[Projectile.owner].Center).ToRotation();

            distanceFromPlayer -= returnSpeed;
            if (distanceFromPlayer <= 20f)
            {
                Projectile.active = false;
            }

            Projectile.position = Main.player[Projectile.owner].Center + directionFromPlayer * distanceFromPlayer;
            Projectile.position -= Projectile.Size / 2;
        }
    }
}
