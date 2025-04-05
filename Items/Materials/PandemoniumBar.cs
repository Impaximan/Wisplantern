using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ObjectData;
using Wisplantern.Items.Placeable.Blocks;

namespace Wisplantern.Items.Materials
{
    class PandemoniumBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Depthrock Block")
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<PandemoniumBarTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient<PandemoniumOre>(9)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }

    class PandemoniumBarTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            DustType = DustID.GreenTorch;
            AddMapEntry(new Color(22, 210, 105), CreateMapEntryName());
        }
    }
}