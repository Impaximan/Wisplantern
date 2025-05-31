using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Equipable.Armor.Hunter
{
    [AutoloadEquip(EquipType.Legs)]
    public class HunterLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<EquipmentPlayer>().huntingWeaponDamage += 0.08f;
            player.moveSpeed *= 1.1f;
        }
    }
}