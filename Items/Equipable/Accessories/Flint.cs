namespace Wisplantern.Items.Equipable.Accessories
{
    class Flint : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.SetScholarlyDescription("Extracted from silt, the flint is a handy stone when it comes to fire");
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);
        }
    }
}
