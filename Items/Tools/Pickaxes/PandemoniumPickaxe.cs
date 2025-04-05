using Terraria.GameContent.Creative;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Tools.Pickaxes
{
    class PandemoniumPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Timid Pick");
            // Tooltip.SetDefault("Gains more speed from hyperstone and wisplanterns than most pickaxes");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.pick = 100;
            Item.useAnimation = 23;
            Item.useTime = 16;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 54, 0);
            Item.knockBack = 2f;
            Item.damage = 12;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
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
