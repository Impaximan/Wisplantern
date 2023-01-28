using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class AerialRetreat : ModItem
    {
        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.AerialRetreat());
        }
    }
}
