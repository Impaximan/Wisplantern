using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Info
{
    class FourLeafClover : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Displays your current luck");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public void AffectPlayer(Player player)
        {
            player.GetModPlayer<LuckInfoPlayer>().fourLeafClover = true;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 18;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.SetScholarlyDescription("Sold on occassion by the Traveling Merchant, a useful tool for keeping up with the superstitious");
        }

        public override void UpdateInventory(Player player)
        {
            AffectPlayer(player);
        }

        public override void UpdateEquip(Player player)
        {
            AffectPlayer(player);
        }
    }

    class LuckInfoPlayer : ModPlayer
    {
        public bool fourLeafClover = false;

        public override void ResetEffects()
        {
            fourLeafClover = false;
        }
    }
    
    class LuckIndicator : InfoDisplay
    {
        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<LuckInfoPlayer>().fourLeafClover;
        }

        public override string DisplayValue(ref Color displayColor)/* tModPorter Suggestion: Set displayColor to InactiveInfoTextColor if your display value is "zero"/shows no valuable information */
        {
            if (Main.LocalPlayer.luck >= 1f)
            {
                return "Dream Luck";
            }
            if (Main.LocalPlayer.luck >= 0.75f)
            {
                return "Incredible luck";
            }
            if (Main.LocalPlayer.luck >= 0.5f)
            {
                return "Great luck";
            }
            if (Main.LocalPlayer.luck >= 0.25f)
            {
                return "Good luck";
            }
            if (Main.LocalPlayer.luck > 0f)
            {
                return "Above average luck";
            }
            if (Main.LocalPlayer.luck < 0f)
            {
                if (Main.LocalPlayer.luck >= -0.14f)
                {
                    return "Below average luck";
                }
                if (Main.LocalPlayer.luck >= -0.42f)
                {
                    return "Bad luck";
                }
                return "Abhorrent luck";
            }
            return "Average luck";
        }
    }
}
