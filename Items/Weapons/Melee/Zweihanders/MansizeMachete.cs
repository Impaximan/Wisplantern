using Terraria.DataStructures;
using Terraria.Audio;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    internal class MansizeMachete : Zweihander
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Perfectly timed strikes guarentee critical hits");
            Item.ResearchUnlockCount = 1;
        }

        public override void ZweihanderDefaults()
        {
            Item.knockBack = 12f;
            Item.width = 74;
            Item.height = 80;
            Item.damage = 37;
            Item.shootSpeed = 9f;
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = DamageClass.Melee;
        }

        public override void OnHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, NPC.HitInfo hit, int damageDone)
        {
            if (perfectCharge)
            {
                int p = Projectile.NewProjectile(Item.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<Bloodsplosion_Machete>(), (int)(damageDone * 0.7f), 0f);
                Main.projectile[p].netUpdate = true;
                SoundEngine.PlaySound(SoundID.Item14, target.Center);
            }
        }

        public override bool HasSwungDust => true;
        public override int SwungDustType => DustID.CrimsonTorch;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 15)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.BloodButcherer);
        }
    }

    class Bloodsplosion_Machete : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Explosives;

        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }

        public override void OnSpawn(IEntitySource source)
        {   
            for (int i = 0; i < 150; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CrimsonTorch, 0, 0, Scale: 2f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = Projectile.DirectionTo(Main.dust[d].position) * Projectile.Distance(Main.dust[d].position) / 15f;
                //Main.dust[d].noLightEmittence = true;
            }
        }
    }
}
