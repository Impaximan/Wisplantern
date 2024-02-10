namespace Wisplantern.Items.BattleArtItems
{
    class RadialCast : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Magic";

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.RadialCast());
        }
    }
}
