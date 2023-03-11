using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
	internal class GoldZweihander : Zweihander
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Perfectly timed strikes guarentee critical hits");
			SacrificeTotal = 1;
		}

		public override void ZweihanderDefaults()
		{
			Item.shootSpeed = 1f;
			Item.knockBack = 9f;
			Item.width = 56;
			Item.height = 56;
			Item.damage = 19;
			Item.shootSpeed = 8.5f;
			Item.rare = ItemRarityID.White;
			Item.DamageType = DamageClass.Melee;
		}

		public override void ModifyHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, ref int damage, ref float knockBack, ref bool crit)
		{
			if (perfectCharge)
			{
				crit = true;
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.GoldBar, 12)
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.Wood, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
