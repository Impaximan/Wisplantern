using Terraria.Audio;

namespace Wisplantern.Items.Weapons.Manipulative.Decoys
{
    class BouncyDummy : DecoyItem
    {
        public override int DecoyType => ModContent.NPCType<BouncyDummyDecoy>();
        public override int CooldownSeconds => 60;
        public override string DecoyName => "bouncy dummy";
        public override int DefaultHP => 500;

        public override void SetStats()
        {
            Item.width = 22;
            Item.height = 36;
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.rare = ItemRarityID.Blue;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(5)
                .AddRecipeGroup(RecipeGroupID.Wood, 10)
                .AddRecipeGroup(RecipeGroupID.IronBar, 2)
                .AddIngredient(ItemID.Hay, 25)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }

    class BouncyDummyDecoy : DecoyNPC
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bouncy Dummy");
            Main.npcFrameCount[Type] = 2;
        }

        public override bool DoContactDamage => false;

        public override void SetStats()
        {
            NPC.width = 36;
            NPC.height = 48;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.HitSound = SoundID.NPCHit15;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.5f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            SoundStyle newStyle = Main.rand.Next(3) switch
            {
                1 => SoundID.NPCHit16,
                2 => SoundID.NPCHit17,
                _ => SoundID.NPCHit15
            };
            
            NPC.HitSound = newStyle;
            NPC.spriteDirection = -hit.HitDirection;
        }

        int timeUntilBounce = 5;
        public override void AI()
        {
            if (NPC.velocity.Y == 0)
            {
                NPC.frame.Y = 48;
                timeUntilBounce--;
                NPC.velocity.X *= 0.93f;
                if (timeUntilBounce <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item56, NPC.Center);
                    NPC.velocity.Y = -10;
                }
            }
            else
            {
                NPC.frame.Y = 0;
                timeUntilBounce = 5;
            }
        }
    }
}
