using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class BloodySlash : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Melee";

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.BloodySlash());
        }
    }
}
