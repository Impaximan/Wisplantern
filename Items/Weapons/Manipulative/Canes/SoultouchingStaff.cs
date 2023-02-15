using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class SoultouchingStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 6)
                .AddIngredient<Placeable.Blocks.Depthrock>(30)
                .AddIngredient(ItemID.Amethyst)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = ModContent.GetInstance<DamageClasses.Manipulative>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = 1;
            Item.width = 30;
            Item.height = 42;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shootSpeed = 1f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.SetManipulativePower(0.1f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        const float maxDistance = 300f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(player.Center) <= maxDistance)
                {
                    Item.AggravateNPC(npc, player);
                }
            }
            for (int i = 0; i < 150; i++)
            {
                Vector2 dustPos = Main.rand.NextVector2CircularEdge(maxDistance, maxDistance) + player.Center;
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.PurpleTorch, player.velocity);
                dust.noGravity = true;
                dust.noLight = true;
            }
            return false;
        }
    }

}
