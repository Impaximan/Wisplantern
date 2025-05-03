using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.CameraModifiers;
using Wisplantern.Globals.GNPCs;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class MoltenCane : CaneWeapon
    {
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 20)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.FieryGreatsword);
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 28;
            Item.SetManipulativePower(0.30f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 58;
            Item.height = 52;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 54, 0);
        }

        public override float MaxDistance => 480f;

        public override int DustType => DustID.Torch;

        public override void OnAggravate(NPC npc, Player player)
        {
            npc.AddBuff(BuffID.Oiled, InfightingNPC.GetAggravationTime(npc));
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Oiled, 60 * 10);

            if (target.life <= 0)
            {

                int p = Projectile.NewProjectile(new EntitySource_OnHit(player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<MoltenCaneExplosion>(), Item.damage, Item.knockBack, player.whoAmI);
            }
        }
    }

    class MoltenCaneExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 60 * 5);
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(255, 70, 25) * 0.01f * (1f - Projectile.frame / 6f));

            if (Projectile.frameCounter == 0 && Projectile.frame == 0)
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);

                PunchCameraModifier modifier = new(Projectile.Center, Main.rand.NextVector2CircularEdge(1f, 1f), 7f, 3f, 10, 1000f);
                Main.instance.CameraModifiers.Add(modifier);
            }

            Projectile.frameCounter++;

            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 6)
                {
                    Projectile.active = false;
                }
            }
        }
    }
}
