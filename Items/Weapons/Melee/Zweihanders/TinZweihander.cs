﻿namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    internal class TinZweihander : Zweihander
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Perfectly timed strikes guarentee critical hits");
			Item.ResearchUnlockCount = 1;
		}

		public override void ZweihanderDefaults()
		{
			Item.knockBack = 9f;
			Item.width = 48;
			Item.height = 48;
			Item.damage = 12;
			Item.shootSpeed = 7f;
			Item.rare = ItemRarityID.White;
			Item.DamageType = DamageClass.Melee;
		}

		public override void ModifyHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, ref NPC.HitModifiers modifiers)
		{
			if (perfectCharge)
            {
                modifiers.SetCrit();
            }
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.TinBar, 16)
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.Wood, 5)
				.AddTile(TileID.Anvils)
				.Register()
                .SortAfterFirstRecipesOf(ItemID.TinBroadsword);
		}
	}
}
