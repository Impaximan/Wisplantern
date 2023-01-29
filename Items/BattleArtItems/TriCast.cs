using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class TriCast : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tri-Cast");
        }

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.TriCast());
        }
    }
}
