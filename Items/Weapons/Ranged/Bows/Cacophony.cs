using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Wisplantern.Items.Materials;
using Microsoft.Xna.Framework.Graphics;

namespace Wisplantern.Items.Weapons.Ranged.Bows
{
    class Cacophony : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;
            Item.width = 22;
            Item.height = 66;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.crit = 10;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.Next(100) < player.GetWeaponCrit(Item))
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    int p = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PandemoniumSpiritRanged>(), damage, knockback, player.whoAmI);
                    //Main.projectile[p].GetGlobalProjectile<FulgariteLongbowArrow>().supercharged = true;
                    Main.projectile[p].ai[0] = type;
                    Main.projectile[p].netUpdate = true;

                    SoundEngine.PlaySound(SoundID.Item73, player.Center);
                }

                return false;
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PandemoniumBar>(20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }


    class PandemoniumSpiritRanged : ModProjectile
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
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
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

        public override void OnKill(int timeLeft)
        {
            int num = 5;
            float offset = Main.rand.NextFloat(MathHelper.TwoPi / num);
            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);

            for (float t = 0; t < MathHelper.TwoPi; t += MathHelper.TwoPi / num)
            {
                Vector2 velocity = (t + offset).ToRotationVector2() * 15f;
                Projectile.NewProjectile(new EntitySource_Misc("Pandemonium Bow Spirit"), Projectile.Center, velocity, (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(125, 255, 255) / 1200f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[1]++;
        }
    }
}
