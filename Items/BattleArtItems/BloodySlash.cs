namespace Wisplantern.Items.BattleArtItems
{
    class BloodySlash : ModItem
    {
        public override string Texture => "Wisplantern/Items/BattleArtItems/Melee";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Deprecated[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.SetAsBattleArtItem(new BattleArts.BloodySlash());
        }
    }
}
