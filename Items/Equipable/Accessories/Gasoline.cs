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
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.SetScholarlyDescription("Bought from the Shepherd NPC.");
            Item.MarkAsShepherd();
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);

            player.GetCritChance<DamageClasses.ManipulativeDamageClass>() += 10;
        }
    }
}
