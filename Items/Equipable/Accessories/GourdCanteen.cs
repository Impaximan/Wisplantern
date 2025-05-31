using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Equipable.Accessories
{
    class GourdCanteen : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);
        }
    }
}
