using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;

namespace Wisplantern.Items.Equipable.Accessories
{
    class Pill : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emergency Pill");
            Tooltip.SetDefault("Greatly increases life regen for a moment when at low health");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 3, 0, 0);
        }

        int lifeRegenTime = 0;
        public override void UpdateEquip(Player player)
        {
            if (player.statLife <= player.statLifeMax2 * 0.2f)
            {
                lifeRegenTime = 300;
            }

            if (lifeRegenTime > 0)
            {
                lifeRegenTime--;
                player.lifeRegen += 10;

                if (Main.rand.NextBool(15))
                {
                    int d = Dust.NewDust(player.position, player.width, player.height, DustID.GreenTorch);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].noLight = true;
                    Main.dust[d].velocity = player.velocity / 5f;
                    Main.dust[d].scale *= 1.5f;
                }
            }
        }
    }
}
