using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Equipable.Accessories
{
    class WispRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Chance when mining to get the 'Hyperspeed' buff for 3 seconds");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<WispRingPlayer>().equipped = true;
        }
    }

    class WispRingPlayer : ModPlayer
    {
        public bool equipped = false;

        public override void ResetEffects()
        {
            equipped = false;
        }
    }

    class WispRingTile : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!Main.gameMenu)
            {
                Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
                if (player != null && player.GetModPlayer<WispRingPlayer>().equipped && !player.HasBuff(ModContent.BuffType<Buffs.Hyperspeed>()))
                {
                    if (!fail && !effectOnly && Main.rand.NextBool(75 - (int)(player.luck * 5f)))
                    {
                        player.AddBuff(ModContent.BuffType<Buffs.Hyperspeed>(), 60 * 3);
                    }
                }
            }
        }
    }
}
