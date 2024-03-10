using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class ShepherdsCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 14;
            Item.SetManipulativePower(0.1f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 36;
            Item.height = 38;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.knockBack = 0f;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 2, 50, 0);
        }

        public override float MaxDistance => 245f;

        public override int DustType => DustID.WoodFurniture;

        public override void OnAggravate(NPC npc, Player player)
        {
            player.SmokeBomb(30);
        }
    }
}
