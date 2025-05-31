using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Wisplantern.DamageClasses;
using Wisplantern.Globals.GItems;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Equipable.Armor.Hunter
{
    [AutoloadEquip(EquipType.Head)]
    public class HunterHelmet : ModItem
    {
        public static LocalizedText SetBonus { get; private set; }

        public override void SetStaticDefaults()
        {
            SetBonus = Mod.GetLocalization($"{nameof(HunterHelmet)}.SetBonus");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<EquipmentPlayer>().huntingWeaponDamage += 0.08f;
            player.GetCritChance(DamageClass.Generic) += 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == Type && body.type == ModContent.ItemType<HunterTunic>() && legs.type == ModContent.ItemType<HunterLeggings>();
        }


        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonus.Value;
            player.AddAccessoryEffect(Item);
        }
    }
}