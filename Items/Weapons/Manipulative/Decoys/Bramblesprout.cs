namespace Wisplantern.Items.Weapons.Manipulative.Decoys
{
    class Bramblesprout : DecoyItem
    {
        public override int DecoyType => ModContent.NPCType<BramblesproutDecoy>();
        public override int CooldownSeconds => 40;
        public override string DecoyName => "bramblesprout";
        public override int DefaultHP => 50;

        public override void SetStats()
        {
            Item.width = 18;
            Item.height = 16;
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.damage = 10;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddRecipeGroup(RecipeGroupID.Wood, 6)
                .AddIngredient(ItemID.Acorn)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    class BramblesproutDecoy : DecoyNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
        }

        public override void SetStats()
        {
            NPC.width = 18;
            NPC.height = 36;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }
    }
}
