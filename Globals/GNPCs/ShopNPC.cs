namespace Wisplantern.Globals.GNPCs
{
    class ShopNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(ModContent.ItemType<Items.Weapons.Melee.Zweihanders.BaseballBat>());
                shop.Add(ModContent.ItemType<Items.Equipable.Accessories.Pill>());
            }

            if (shop.NpcType == NPCID.TravellingMerchant)
            {
                shop.Add(ModContent.ItemType<Items.Info.FourLeafClover>(), Condition.MoonPhasesEven);
                shop.Add(ModContent.ItemType<Items.Equipable.Accessories.GlintstoneGlove>(), Condition.MoonPhasesOdd);
            }

            if (shop.NpcType == NPCID.SkeletonMerchant)
            {
                shop.Add(ModContent.ItemType<Items.Equipable.Accessories.GlintstoneGlove>(), Condition.MoonPhasesOdd);
            }
        }
    }
}