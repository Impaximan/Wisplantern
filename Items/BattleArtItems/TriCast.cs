namespace Wisplantern.Items.BattleArtItems
{
    class TriCast : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Magic";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tri-Cast");
        }

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.TriCast());
        }
    }
}
