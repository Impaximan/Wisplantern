namespace Wisplantern.Globals.GItems
{
    class ExtractinatorItem : GlobalItem
    {
        //Putting this here because I don't feel like making a whole other GlobalItem for this
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Snowball)
            {
                item.notAmmo = true;
            }
        }

        public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            if (extractType == 0)
            {
                if (Main.rand.NextBool(50))
                {
                    resultType = ModContent.ItemType<Items.Equipable.Accessories.Flint>();
                    resultStack = 1;
                }
            }
        }
    }
}
