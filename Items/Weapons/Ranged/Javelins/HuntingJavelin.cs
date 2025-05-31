using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Wisplantern.Items.Equipable.Armor.Hunter;
using Wisplantern.Items.Weapons.Melee.Shortswords;

namespace Wisplantern.Items.Weapons.Ranged.Javelins
{
    class HuntingJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.shoot = ModContent.ProjectileType<HuntingJavelinProjectile>();
            Item.shootSpeed = 25f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.width = 38;
            Item.height = 38;
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 0, 0, 50);
            Item.MarkAsHuntingWeapon();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.Y -= 8;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            if (player.AccessoryActive<HunterHelmet>())
            {
                foreach (int buff in player.buffType)
                {
                    if (BuffID.Sets.IsWellFed[buff])
                    {
                        return 1.5f;
                    }
                }
            }
            return base.UseSpeedMultiplier(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddRecipeGroup(RecipeGroupID.Wood, 3)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class HuntingJavelinProjectile : ModProjectile
    {
        int numHits = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.hostile = false;
            numHits = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.OffsetSize(-15, -15);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return (pauseTime > 0) ? false : null;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            pauseTime += 6;
            numHits++;
            SoundStyle style = new("Wisplantern/Sounds/Effects/SharpHit2");
            style.PitchVariance = 0.35f;
            style.Pitch -= 0.35f;
            style.Volume = 0.75f;
            style.MaxInstances = 1;
            SoundEngine.PlaySound(style, target.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            return base.OnTileCollide(oldVelocity);
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(Math.Clamp(5 - numHits * 2, 1, 5)))
            {
                Item.NewItem(new EntitySource_DropAsItem(Projectile), Projectile.getRect(), ModContent.ItemType<HuntingJavelin>());
            }
            else if (Main.netMode != NetmodeID.Server)
            {
                for (float i = -1f; i < 1f; i += 1f / 10f)
                {
                    Vector2 position = Projectile.Center + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * (i * (float)Math.Sqrt(Projectile.width * Projectile.height) - 35f) + Main.rand.NextVector2Circular(3f, 3f);
                    Dust d = Dust.NewDustPerfect(position, DustID.DynastyWood, Projectile.velocity / 10f, Scale: (i + 2f) * 0.5f);
                    d.noGravity = true;
                }
            }
        }

        int pauseTime = 0;
        int gravityCounter = 0;
        public override void AI()
        {
            if (pauseTime > 0)
            {
                pauseTime--;
                Projectile.position -= Projectile.velocity;
            }
            else
            {
                gravityCounter++;
                if (gravityCounter > 10) Projectile.velocity.Y += 0.4f;
                Projectile.velocity *= 0.98f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi / 4f;
            }
        }
    }
}
