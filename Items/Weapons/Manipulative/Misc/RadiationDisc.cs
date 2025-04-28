using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Wisplantern.DamageClasses;

namespace Wisplantern.Items.Weapons.Manipulative.Misc
{
    public class RadiationDisc : ModItem
    {
        public override bool WeaponPrefix()
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.SetManipulativePower(0.18f);
            Item.DamageType = ModContent.GetInstance<ManipulativeDamageClass>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<RadiationDiscProjectile>();
            Item.width = 28;
            Item.noUseGraphic = true;
            Item.height = 28;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.value = Item.sellPrice(0, 0, 35, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class RadiationDiscProjectile : ModProjectile
    {
        public override string Texture => "Wisplantern/Items/Weapons/Manipulative/Misc/RadiationDisc";
        public override string GlowTexture => "Wisplantern/Items/Weapons/Manipulative/Misc/RadiationDisc_Glow";

        const int time = 180;

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.DamageType = ModContent.GetInstance<ManipulativeDamageClass>();
            Projectile.timeLeft = time;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.ignoreWater = false;
            Projectile.ai[0] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D glow = ModContent.Request<Texture2D>("Wisplantern/VFX/Shine1").Value;
            int radius = (int)Projectile.ai[0] + (int)(Math.Sin(Projectile.timeLeft / 18f) * 10f) + 25;
            Rectangle rect = new Rectangle((int)Projectile.Center.X - radius - (int)Main.screenPosition.X, (int)Projectile.Center.Y - radius - (int)Main.screenPosition.Y, radius * 2, radius * 2);

            for (int i = 0; i < 5; i++)
            {
                Main.spriteBatch.Draw(glow, rect, Color.Orange * 0.35f * (1f - Projectile.alpha / 255f));
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            return base.PreDraw(ref lightColor);
        }

        Item? playerItem;

        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse sauce)
            {
                playerItem = sauce.Item;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X == 0)
            {
                Projectile.velocity.X = oldVelocity.X * -1;
            }

            if (Projectile.velocity.Y == 0)
            {
                Projectile.velocity.Y = oldVelocity.Y * -1;
            }

            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, new Vector3(Color.Orange.R * 0.1f, Color.Orange.G * 0.1f, Color.Orange.B * 0.1f) * Projectile.ai[0] / 5000f);

            if (Projectile.timeLeft == time)
            {
                float rotation = 0f;
                float virtualVelocity = Projectile.velocity.Length();

                for (int i = 0; i < time; i++)
                {
                    rotation += virtualVelocity / 6f;
                    virtualVelocity *= 0.93f;
                }

                Projectile.rotation -= rotation * Math.Sign(Projectile.velocity.X);
            }

            Projectile.rotation += Math.Sign(Projectile.velocity.X) * Projectile.velocity.Length() / 6f;
            Projectile.velocity *= 0.93f;

            Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 150f, 0.02f);

            if (Projectile.timeLeft < 255 / 15)
            {
                Projectile.alpha += 15;
            }


            if (Main.myPlayer == Projectile.owner)
            {
                int radius = (int)Projectile.ai[0];
                Rectangle rect = new Rectangle((int)Projectile.Center.X - radius, (int)Projectile.Center.Y - radius, radius * 2, radius * 2);

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.Hitbox.Intersects(rect))
                    {
                        if (playerItem != null)
                        {
                            if (Projectile.IsNPCIndexImmuneToProjectileType(Type, npc.whoAmI))
                            {
                                npc.Aggravate(playerItem, Main.player[Projectile.owner]);
                                npc.immune[Projectile.owner] = 0;
                                Projectile.perIDStaticNPCImmunity[Type][npc.whoAmI] = Main.GameUpdateCount + (uint)Projectile.idStaticNPCHitCooldown;
                            }
                        }
                    }
                }
            }
        }
    }
}
