using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class BloodySlash : ModItem
    {
        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.BloodySlash());
        }
    }
}
