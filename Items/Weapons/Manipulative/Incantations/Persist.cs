using Terraria.DataStructures;
using Wisplantern.Buffs;
using Wisplantern.Globals.GNPCs;

namespace Wisplantern.Items.Weapons.Manipulative.Incantations
{
    public class Persist : Incantation
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.ScrollOfIncantation>()
                .AddIngredient(ItemID.Vertebrae, 5)
                .AddIngredient(ItemID.CrimtaneBar, 4)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override void IncantationSetDefaults()
        {
            Item.UseSound = SoundID.DD2_DarkMageSummonSkeleton;
            Item.rare = ItemRarityID.Blue;
            Item.SetCharisma(1);
            Item.knockBack = 0f;
        }

        public override void IncantationEffect(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 targetPosition, NPC npc, int damage)
        {
            if (npc.TryGetGlobalNPC(out InfightingNPC iNPC))
            {
                iNPC.aggravation = 1f;
                int healAmount = Math.Clamp(npc.lifeMax - npc.life, 0, 50);
                npc.HealEffect(healAmount);
                npc.life += healAmount;
                iNPC.infightDamage += 5;
            }
        }
    }
}
