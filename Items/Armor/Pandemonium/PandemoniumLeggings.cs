using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Armor.Pandemonium
{
    [AutoloadEquip(EquipType.Legs)]
    public class PandemoniumLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //ArmorIDs.Body.Sets.HidesTopSkin[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body)] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
            Item.MarkAsShepherd();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ManipulativePlayer>().extraMaxCharisma++;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PandemoniumBar>(20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}