﻿using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class CopperCane : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBar, 10)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.CopperBroadsword);
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 9;
            Item.SetManipulativePower(0.135f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 235f;

        public override int DustType => DustID.Copper;
    }
}
