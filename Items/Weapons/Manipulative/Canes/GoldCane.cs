using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class GoldCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 16;
            Item.SetManipulativePower(0.16f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 265f;

        public override int DustType => DustID.GoldCoin;
    }
}
