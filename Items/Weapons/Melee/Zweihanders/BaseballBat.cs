using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    internal class BaseballBat : Zweihander
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Perfectly timed strikes have thrice as much knockback");
			Item.ResearchUnlockCount = 1;
		}

		public override void ZweihanderDefaults()
		{
			Item.knockBack = 7.5f;
			Item.width = 46;
			Item.height = 46;
			Item.damage = 20;
			Item.shootSpeed = 3f;
			Item.crit = 8;
			Item.rare = ItemRarityID.Blue;
			Item.DamageType = DamageClass.Melee;
			Item.UseSound = SoundID.Item8;
			Item.value = Item.buyPrice(0, 5, 0, 0);
		}

        public override int ChargeTime => 90;

        public override void ModifyHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, ref NPC.HitModifiers modifiers)
		{
			if (perfectCharge)
			{
				modifiers.Knockback *= 3f;

				if (firstHit)
				{
					player.GetModPlayer<BaseballBatPlayer>().totalHomeRuns++;
					CombatText.NewText(target.getRect(), Color.Orange, Main.rand.Next(new List<string>()
					{
						"HOME RUN!",
						"Nice swing!",
						"A swing... and a hit!",
						player.name + " makes yet another beautiful hit!",
						"Yet another home run for " + player.name + "'s collection...",
						"The legend strikes (not out) once again!",
						"Home run number " + player.GetModPlayer<BaseballBatPlayer>().totalHomeRuns + " for " + player.name + "!",
					}));
				}
			}
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
			tooltips.Add(new TooltipLine(Mod, "BaseballBat", "Total number of 'home runs': [c/ffa500:" + Main.LocalPlayer.GetModPlayer<BaseballBatPlayer>().totalHomeRuns + "]"));
        }
    }

	internal class BaseballBatPlayer : ModPlayer
    {
		public int totalHomeRuns = 0;

        public override void SaveData(TagCompound tag)
        {
			tag.Add("totalHomeRuns", totalHomeRuns);
        }

        public override void LoadData(TagCompound tag)
        {
			totalHomeRuns = tag.GetInt("totalHomeRuns");
        }
    }
}

