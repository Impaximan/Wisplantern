using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class PlatinumCane : CaneWeapon
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
            Item.damage = 17;
            Item.SetManipulativePower(0.16f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.Manipulative>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 270f;

        public override int DustType => DustID.PlatinumCoin;
    }
}
