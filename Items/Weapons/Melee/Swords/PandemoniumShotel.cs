using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Weapons.Melee.Swords
{
    class PandemoniumShotel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Timid Pick");
            // Tooltip.SetDefault("Gains more speed from hyperstone and wisplanterns than most pickaxes");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 37;
            Item.useAnimation = 11;
            Item.useTime = 11;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 54, 0);
            Item.knockBack = 5f;
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.useTurn = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PandemoniumBar>(20)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PandemoniumSpirit>()] < 15)
            {
                int p = Projectile.NewProjectile(new EntitySource_OnHit(player, target, "Pandemonium Shotel"), target.Center, Vector2.Zero, ModContent.ProjectileType<PandemoniumSpirit>(), (int)(Item.damage * 2f), Item.knockBack, player.whoAmI);
                Main.projectile[p].ai[1] = Main.rand.NextFloat(100f);
                Main.projectile[p].netUpdate = true;
                player.GetModPlayer<EquipmentPlayer>().pandemoniumSouls.Add(Main.projectile[p]);
                SoundEngine.PlaySound(SoundID.NPCHit36, target.Center);

                bool maxed = player.ownedProjectileCounts[ModContent.ProjectileType<PandemoniumSpirit>()] + 1 == 15;
                CombatText.NewText(target.getRect(), maxed ? Color.LightCyan : Color.DarkCyan, player.ownedProjectileCounts[ModContent.ProjectileType<PandemoniumSpirit>()] + 1, maxed);
                if (maxed) player.DoManaRechargeEffect();
            }
        }
    }

    class PandemoniumSpirit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Lerp(Color.White, Color.Blue, 1f - ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 14;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(-8, 0);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] == 0)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity * 0.75f;
            Projectile.tileCollide = false;

            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);
            Projectile.ai[0] = 3;

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 3)
            {
                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);
                Projectile.ai[0] = 3;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Lighting.AddLight(Projectile.Center, new Vector3(125, 255, 255) / 1200f);

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (player.dead)
            {
                Projectile.active = false;
            }

            if (Projectile.ai[0] == 0)
            {
                Projectile.timeLeft = 600;
                Projectile.ai[1]++;

                Vector2 target = player.Center + new Vector2(player.direction * -25f, -30f) + (float)Math.Sin(Projectile.ai[1] / 14f) * 50f * (Projectile.ai[1] / 50f).ToRotationVector2();
                Projectile.velocity = (target - Projectile.Center) / 10f;
            }
            else if (Projectile.ai[0] == 1)
            {
                SoundEngine.PlaySound(SoundID.Item73, Projectile.Center);

                if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.Center = player.Center;
                    Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * Projectile.velocity.Length();
                    Projectile.netUpdate = true;
                }

                Projectile.tileCollide = true;
                Projectile.ai[0]++;
            }
            else if (Projectile.ai[0] == 3)
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
