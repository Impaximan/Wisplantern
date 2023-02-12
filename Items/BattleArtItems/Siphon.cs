using Terraria;
using Terraria.ModLoader;

namespace Wisplantern.Items.BattleArtItems
{
    class Siphon : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Magic";

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.Siphon());
        }
    }
}
