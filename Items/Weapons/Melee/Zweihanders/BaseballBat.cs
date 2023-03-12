using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
	internal class BaseballBat : Zweihander
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Perfectly timed strikes have thrice as much knockback");
			SacrificeTotal = 1;
		}

		public override void ZweihanderDefaults()
		{
			Item.knockBack = 7.5f;
			Item.width = 46;
			Item.height = 46;
			Item.damage = 20;
			Item.shootSpeed = 3f;
			Item.crit = 8;
			Item.rare = ItemRarityID.White;
			Item.DamageType = DamageClass.Melee;
			Item.UseSound = SoundID.Item8;
			Item.value = Item.buyPrice(0, 5, 0, 0);
		}

        public override int ChargeTime => 90;

        public override void ModifyHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, ref int damage, ref float knockBack, ref bool crit)
		{
			if (perfectCharge)
			{
				knockBack *= 3f;

				if (firstHit)
				{
					CombatText.NewText(target.getRect(), Color.Orange, Main.rand.Next(new List<string>()
					{
						"HOME RUN!",
						"Nice swing!",
						"A swing... and a hit!",
						player.name + " makes yet another beautiful hit!",
						"Yet another home run for " + player.name + "'s collection...",
						"The legend strikes (not out) once again!"
					}));
				}
			}
		}
	}
}

