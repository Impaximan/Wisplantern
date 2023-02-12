using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class AerialRetreat : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Ranged";

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.AerialRetreat());
        }
    }
}
