using Terraria.GameContent.Creative;
using Wisplantern.Items.Weapons.Magic.Staffs;

namespace Wisplantern.Items.Equipable.Accessories
{
    class GlintstoneGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Allows you to swing magic staffs, recovering mana on melee hits" +
                "\n+20 max mana"); */
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
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.SetScholarlyDescription("Sold by undead merchants underground and travelling merchants during certain moon phases");
        }

        public override void UpdateEquip(Player player)
        {
            player.AddAccessoryEffect(Item);
            player.statManaMax2 += 20;
        }
    }
}
