namespace Wisplantern.Items.BattleArtItems
{
    class FocusShot : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Ranged";

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.FocusShot());
        }
    }
}
