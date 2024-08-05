using Terraria.DataStructures;
using Wisplantern.Buffs;

namespace Wisplantern.Items.Weapons.Manipulative.Incantations
{
    public class ShowNoMercy : Incantation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.ScrollOfIncantation>()
                .AddIngredient(ItemID.WormTooth, 3)
                .AddIngredient(ItemID.DemoniteBar, 4)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override void IncantationSetDefaults()
        {
            Item.UseSound = SoundID.DD2_DarkMageSummonSkeleton;
            Item.rare = ItemRarityID.Blue;
            Item.SetCharisma(3);
            Item.damage = 1;
            Item.knockBack = 0f;
        }

        public override void IncantationEffect(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 targetPosition, NPC npc, int damage)
        {
            npc.EvenSimplerStrikeNPC(player, Item, damage, 0f, 0);

            npc.AddBuff(ModContent.BuffType<VisciousFrenzy>(), 8 * 60);
        }
    }
}
