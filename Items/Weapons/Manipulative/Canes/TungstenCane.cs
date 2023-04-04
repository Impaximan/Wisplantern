using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class TungstenCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 14;
            Item.SetManipulativePower(0.15f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.Manipulative>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 260f;

        public override int DustType => DustID.Tungsten;
    }
}
