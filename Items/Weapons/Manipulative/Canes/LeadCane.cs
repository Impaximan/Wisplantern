using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class LeadCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 13;
            Item.SetManipulativePower(0.1425f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 250f;

        public override int DustType => DustID.Lead;
    }
}
