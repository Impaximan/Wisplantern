namespace Wisplantern.Items.Equipable.Accessories
{
    class Gasoline : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 24;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.SetScholarlyDescription("Found in random chests underground.");
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);

            player.GetCritChance<DamageClasses.ManipulativeDamageClass>() += 10;
        }
    }
}
