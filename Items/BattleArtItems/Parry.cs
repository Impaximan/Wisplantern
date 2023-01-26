using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class Parry : ModItem
    {
        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.Parry());
        }

        public override bool CanRightClick()
        {
            return true;
        }
    }
}
