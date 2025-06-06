﻿using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class SilverCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 10)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.SilverBroadsword);
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 13;
            Item.SetManipulativePower(0.15f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 255f;

        public override int DustType => DustID.Silver;
    }
}
