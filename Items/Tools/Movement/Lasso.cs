
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Wisplantern.Items.Tools.Movement
{
    public class Lasso : ModItem
    {
        public override void SetDefaults()
        {
            Item.ResearchUnlockCount = 50;
            Item.width = 36;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.noUseGraphic = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LassoProjectile>();
            Item.shootSpeed = 25f;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.SetScholarlyDescription("Bought from the Shepherd");
            Item.consumable = true;
        }

        int cooldown = 0;
        public override void UpdateInventory(Player player)
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (cooldown > 0)
            {
                return false;
            }

            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.owner == player.whoAmI && projectile.type == Item.shoot && projectile.active)
                {
                    if (projectile.ai[1] == 1f)
                    {
                        cooldown = 20;
                        projectile.active = false;
                        Item.NewItem(new EntitySource_DropAsItem(projectile), projectile.Center, Type);
                    }
                    return false;
                }
            }

            return base.CanUseItem(player);
        }
    }

    public class LassoProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        NPC attachedNPC = null;
        public override void SetDefaults()
        {
            Projectile.timeLeft = 120;
            Projectile.width = 34;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.ai[0] = 0f;
            Projectile.ai[1] = 0f;
            attachedNPC = null;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 5;
            height = 5;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        float rotation = 0f;
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 originPoint = Main.player[Projectile.owner].Center;
            Vector2 center = Projectile.Center + rotation.ToRotationVector2() * (attachedNPC != null ? attachedNPC.Size / 2 : new Vector2(14, 4));
            Vector2 distToProj = originPoint - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = ModContent.Request<Texture2D>("Wisplantern/Items/Tools/Movement/LassoChain").Value;

            while (distance > texture.Height && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= texture.Width;
                center += distToProj;
                distToProj = originPoint - center;
                distance = distToProj.Length();


                //Draw chain
                Main.spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projRotation + MathHelper.PiOver2,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }

            return attachedNPC == null;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity * -1;
            travelTime = 30;
            return false;
        }

        int travelTime = 0;
        public override void AI()
        {
            if (attachedNPC == null)
            {
                rotation += MathHelper.TwoPi / 20f;

                travelTime++;

                if (travelTime > 30)
                {
                    Projectile.velocity = Projectile.velocity.Length() * Projectile.DirectionTo(Main.player[Projectile.owner].Center);

                    if (Projectile.Distance(Main.player[Projectile.owner].Center) <= Projectile.velocity.Length() * 2f)
                    {
                        Projectile.active = false;
                        Item.NewItem(new EntitySource_DropAsItem(Projectile), Projectile.Center, ModContent.ItemType<Lasso>());
                    }
                    Projectile.tileCollide = false;
                }

                foreach (NPC npc in Main.npc)
                {
                    if (npc.getRect().Intersects(Projectile.getRect()) && npc.knockBackResist > 0f && !npc.boss && npc.active)
                    {
                        attachedNPC = npc;
                        Projectile.ai[0] = Main.player[Projectile.owner].Distance(npc.Center);
                        SoundEngine.PlaySound(npc.HitSound, npc.Center);
                        Projectile.ai[1] = 1f;
                        Projectile.tileCollide = false;
                        break;
                    }
                }

                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 10)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame > 1)
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            else
            {
                rotation = Projectile.DirectionTo(Main.player[Projectile.owner].Center).ToRotation();
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = attachedNPC.Center;
                Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 300f, 0.05f);
                Projectile.timeLeft = 2;

                float dist = attachedNPC.Distance(Main.player[Projectile.owner].Center);
                if (dist > Projectile.ai[0])
                {
                    attachedNPC.velocity += attachedNPC.DirectionTo(Main.player[Projectile.owner].Center) * (dist - Projectile.ai[0]) * 0.03f;
                    if (dist > Projectile.ai[0] * 1.5f)
                    {
                        Item.NewItem(new EntitySource_DropAsItem(Projectile), Projectile.Center, ModContent.ItemType<Lasso>());
                        Projectile.active = false;
                    }
                }

                if (!attachedNPC.active)
                {
                    Item.NewItem(new EntitySource_DropAsItem(Projectile), Projectile.Center, ModContent.ItemType<Lasso>());
                    Projectile.active = false;
                }
            }
        }
    }
}
