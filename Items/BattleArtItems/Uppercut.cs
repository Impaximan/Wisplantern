using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class Uppercut : ModItem
    {
        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.Uppercut());
        }
    }
}
