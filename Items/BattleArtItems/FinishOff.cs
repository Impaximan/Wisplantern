using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class FinishOff : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Manipulative";

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.FinishOff());
        }
    }
}
