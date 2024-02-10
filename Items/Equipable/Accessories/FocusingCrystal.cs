using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Equipable.Accessories
{
    class FocusingCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Critical strikes are more deadly" +
                "\n5% increased critical strike chance"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);
            player.GetCritChance(DamageClass.Generic) += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Placeable.Blocks.Fulgarite>(25)
                .AddIngredient(ItemID.Sapphire, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
