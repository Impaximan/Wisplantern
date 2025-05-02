using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Materials
{
    class Pyrite : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Depthrock Block");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.createTile = ModContent.TileType<Tiles.Pyrite>();
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe.Create(ItemID.Torch, 8)
                .AddIngredient(Type)
                .AddRecipeGroup(RecipeGroupID.Wood)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.Torch);
        }
    }
}
