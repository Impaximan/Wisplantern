using Terraria.DataStructures;

namespace Wisplantern.Items.Weapons.Manipulative.Incantations
{
    public class WontYouBurnForMe : Incantation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.ScrollOfIncantation>()
                .AddRecipeGroup(RecipeGroupID.Wood, 5)
                .AddIngredient(ItemID.Gel, 10)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override void IncantationSetDefaults()
        {
            Item.UseSound = SoundID.DD2_BetsysWrathImpact;
            Item.rare = ItemRarityID.Blue;
            Item.SetCharisma(2);
            Item.damage = 15;
            Item.knockBack = 0f;
        }

        public override void IncantationEffect(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 targetPosition, NPC npc, int damage)
        {
            npc.EvenSimplerStrikeNPC(player, Item, damage, 0f, 0);
            npc.AddBuff(BuffID.OnFire3, 200);
        }
    }
}
