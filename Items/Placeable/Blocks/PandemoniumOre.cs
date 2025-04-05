using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Placeable.Blocks
{
    class PandemoniumOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Depthrock Block");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 0, 2, 50);
            Item.createTile = ModContent.TileType<Tiles.PandemoniumOre>();
            Item.rare = ItemRarityID.Green;
        }
    }
}
