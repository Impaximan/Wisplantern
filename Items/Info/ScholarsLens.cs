using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Info
{
    class ScholarsLens : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Scholar's Lens");
            // Tooltip.SetDefault("Shows extra information on how to obtain items and more within their tooltips when equipped");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.SetScholarlyDescription("The very item allowing you to see this golden tooltip" +
                "\n'An article writer's dream!'");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ScholarlyPlayer>().equipped = true;
        }
    }


    class ScholarlyPlayer : ModPlayer
    {
        public bool equipped = false;

        public override void ResetEffects()
        {
            equipped = false;
        }
    }
}
