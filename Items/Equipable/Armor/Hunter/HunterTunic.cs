using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Equipable.Armor.Hunter
{
    [AutoloadEquip(EquipType.Body)]
    public class HunterTunic : ModItem
    {
        public static int scarf;

        public override void Load()
        {
            scarf = EquipLoader.AddEquipTexture(Mod, "Wisplantern/Items/Equipable/Armor/Hunter/HunterTunic_Scarf", EquipType.Front, name: Name);
        }
        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.IncludedCapeFront[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body)] = scarf;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<EquipmentPlayer>().huntingWeaponDamage += 0.14f;
        }
    }
}