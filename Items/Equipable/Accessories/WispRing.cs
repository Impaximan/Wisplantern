﻿using Terraria.GameContent.Creative;

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
            Item.SetScholarlyDescription("Found mysteriously from the mystical remains of Wisplanterns underground");
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);
        }
    }

    class WispRingTile : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!Main.gameMenu)
            {
                Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
                if (player != null && player.AccessoryActive<WispRing>() && !player.HasBuff(ModContent.BuffType<Buffs.Hyperspeed>()))
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
