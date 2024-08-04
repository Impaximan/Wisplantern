using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Materials
{
    class ScrollOfIncantation : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Depthrock Block")
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 15;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.SetScholarlyDescription("Bought from the Shepherd NPC.");
            Item.MarkAsShepherd();
        }
    }
}

