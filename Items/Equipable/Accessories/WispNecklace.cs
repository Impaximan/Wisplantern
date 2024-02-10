using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Equipable.Accessories
{
    class WispNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Weapons benefit from mining speed" +
                "\n10% increased mining speed"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 18;
            Item.height = 26;
            Item.defense = 1;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.SetScholarlyDescription("Found mysteriously from the mystical remains of Wisplanterns underground");
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);
            player.pickSpeed *= 0.9f;
        }
    }
}
