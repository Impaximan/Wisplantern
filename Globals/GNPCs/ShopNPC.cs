using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Wisplantern.Globals.GNPCs
{
    class ShopNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Zweihanders.BaseballBat>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.Pill>());
                nextSlot++;
            }

            if (type == NPCID.TravellingMerchant)
            {
                if (Main.moonPhase % 3 == 0)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Info.FourLeafClover>());
                    nextSlot++;
                }
            }

            if (type == NPCID.SkeletonMerchant)
            {
                if (Main.moonPhase % 2 == 0)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.GlintstoneGlove>());
                    nextSlot++;
                }
            }
        }
    }
}