using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System.Runtime.InteropServices;
using Wisplantern.Globals.GNPCs;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class EnchantedCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("Causes nearby fallen stars to gravitate towards aggravated enemies");
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WoodenCane>()
                .AddIngredient(ItemID.GoldBar, 10)
                .AddIngredient(ItemID.FallenStar, 8)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient<WoodenCane>()
                .AddIngredient(ItemID.PlatinumBar, 10)
                .AddIngredient(ItemID.FallenStar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 11;
            Item.SetManipulativePower(0.075f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.Manipulative>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 350f;

        public override int DustType
        {
            get
            {
                return Main.rand.Next(0) switch
                {
                    0 => 15,
                    _ => 0,
                };
            }
        }

        public override void OnAggravate(NPC npc)
        {
            npc.GetGlobalNPC<EnchantedCaneNPC>().affected = true;
        }
    }

    class EnchantedCaneNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool affected = false;

        public override void PostAI(NPC npc)
        {
            InfightingNPC iNPC = npc.GetGlobalNPC<InfightingNPC>();
            if (!iNPC.aggravated)
            {
                affected = false;
            }
            else if (affected)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (projectile.active && projectile.type == ProjectileID.FallingStar && projectile.Distance(npc.Center) < 5000f)
                    {
                        projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(npc.Center).ToRotation(), MathHelper.ToRadians(5f)).ToRotationVector2() * projectile.velocity.Length();
                    }
                }
            }
        }
    }
}
