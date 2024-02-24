using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class CrimtaneCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 20;
            Item.SetManipulativePower(0.225f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 46;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 380f;

        public override int DustType => DustID.Blood;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
            {
                Projectile.NewProjectile(new EntitySource_OnHit(player, target, "Crimtane cane hit"), target.Center, target.velocity, ModContent.ProjectileType<CrimtaneSkull>(), (int)player.GetDamage(ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>()).ApplyTo(10f), 0.1f, player.whoAmI);
            }
        }
    }

    class CrimtaneSkull : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 26;
            Projectile.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.alpha = 50;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 300f)
            {
                Projectile.alpha += 5;

                if (Projectile.alpha > 255)
                {
                    Projectile.active = false;
                }
            }

            Projectile.velocity *= 0.95f;

            Projectile.frame = (Projectile.ai[0] % 60 < 30) ? 1 : 0;

            Projectile.scale = 1f + (float)Math.Sin(Projectile.ai[0] / 15f) / 5f;

            NPC target = null;
            float distance = 50f;

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

                Projectile.velocity += Projectile.DirectionTo(target.Center) * 0.25f;
            }
        }
    }
}
