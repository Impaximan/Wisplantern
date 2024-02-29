using Terraria.DataStructures;

namespace Wisplantern.Items.Weapons.Manipulative.Incantations
{
    public class EatYourHeartOut : Incantation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.ScrollOfIncantation>()
                .AddIngredient(ItemID.LifeCrystal)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override void IncantationSetDefaults()
        {
            Item.UseSound = SoundID.Item2;
            Item.rare = ItemRarityID.Blue;
            Item.SetCharisma(1);
            Item.damage = 17;
            Item.knockBack = 0f;
        }

        public override void IncantationEffect(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 targetPosition, NPC npc, int damage)
        {
            npc.EvenSimplerStrikeNPC(player, Item, damage, 0f, 0);

            Item.NewItem(new EntitySource_OnHit(player, npc), npc.Center, ItemID.Heart);
        }
    }
}
