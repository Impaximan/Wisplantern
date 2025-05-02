using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class WoodenCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 11)
                .AddTile(TileID.WorkBenches)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.WoodenSword);
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 25);
            Item.SetManipulativePower(0.13f);
        }

        public override float MaxDistance => 225f;

        public override int DustType => DustID.WoodFurniture;
    }
}
