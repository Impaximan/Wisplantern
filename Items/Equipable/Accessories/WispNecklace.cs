using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Equipable.Accessories
{
    class WispNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Weapons benefit from mining speed" +
                "\n10% increased mining speed"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 18;
            Item.height = 26;
            Item.defense = 1;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<WispNecklacePlayer>().equipped = true;
            player.pickSpeed *= 0.9f;
        }
    }

    class WispNecklacePlayer : ModPlayer
    {
        public bool equipped = false;

        public override void ResetEffects()
        {
            equipped = false;
        }

        public override void PostUpdateEquips()
        {
            if (equipped)
            {
                float usedPickSpeed = Player.pickSpeed;
                if (Player.HasBuff(ModContent.BuffType<Buffs.Hyperspeed>()))
                {
                    if (usedPickSpeed < 0.5f) usedPickSpeed = 0.5f;
                }
                else
                {
                    if (usedPickSpeed < 0.75f) usedPickSpeed = 0.75f;
                }
                Player.GetAttackSpeed(DamageClass.Generic) *= MathHelper.Lerp((1f / usedPickSpeed), 1f, 0.5f);
            }
        }
    }
}
