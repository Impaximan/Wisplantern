using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Armor.Pandemonium
{
    [AutoloadEquip(EquipType.Body)]
    public class PandemoniumBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.HidesTopSkin[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 6;
            Item.MarkAsShepherd();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ManipulativePlayer>().manipulativePower += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PandemoniumBar>(30)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}