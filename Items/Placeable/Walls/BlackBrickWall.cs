using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Placeable.Walls
{
    class BlackBrickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Depthrock Block");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient<Blocks.BlackBrick>()
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<Tiles.BlackBrickWall>();
        }
    }
}
