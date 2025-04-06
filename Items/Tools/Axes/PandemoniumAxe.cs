using Terraria.GameContent.Creative;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Tools.Axes
{
    class PandemoniumAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Timid Pick");
            // Tooltip.SetDefault("Gains more speed from hyperstone and wisplanterns than most pickaxes");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 40;
            Item.axe = 30;
            Item.useAnimation = 23;
            Item.useTime = 12;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 54, 0);
            Item.knockBack = 2f;
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.useTurn = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PandemoniumBar>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
