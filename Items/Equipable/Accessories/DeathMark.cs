namespace Wisplantern.Items.Equipable.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class DeathMark : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 30;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 15, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);
        }
    }
}
