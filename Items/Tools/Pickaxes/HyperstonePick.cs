using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;

namespace Wisplantern.Items.Tools.Pickaxes
{
    class HyperstonePick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Timid Pick");
            Tooltip.SetDefault("Gains more speed from hyperstone and wisplanterns than most pickaxes");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;
            Item.pick = 55;
            Item.useAnimation = 16;
            Item.useTime = 13;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.knockBack = 2f;
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void HoldItem(Player player)
        {
            if (player.HasBuff<Buffs.Hyperspeed>())
            {
                player.pickSpeed *= 0.65f;
            }
        }
    }
}
