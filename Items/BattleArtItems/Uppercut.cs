namespace Wisplantern.Items.BattleArtItems
{
    class Uppercut : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Melee";

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.Uppercut());
        }
    }
}
