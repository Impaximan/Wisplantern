//using Terraria;
//using Terraria.ModLoader;
//using Terraria.ID;
//using Terraria.GameContent.ItemDropRules;
//using Wisplantern.Globals.GNPCs;
//using Terraria.DataStructures;
//using Microsoft.Xna.Framework;

//namespace Wisplantern.Items.Weapons.Manipulative.Decoys
//{
//    class Bramblesprout : DecoyItem
//    {
//        public override int DecoyType => ModContent.NPCType<BramblesproutDecoy>();

//        public override void SetStats()
//        {
//            Item.width = 18;
//            Item.height = 16;
//            Item.DamageType = ModContent.GetInstance<DamageClasses.Manipulative>();
//            Item.damage = 10;
//            Item.consumable = true;
//            Item.maxStack = 9999;
//            Item.useTime = 30;
//            Item.useAnimation = 30;
//            Item.useStyle = ItemUseStyleID.Swing;
//        }

//        public override void AddRecipes()
//        {
//            CreateRecipe(3)
//                .AddRecipeGroup(RecipeGroupID.Wood, 5)
//                .AddIngredient(ItemID.Acorn)
//                .AddTile(TileID.WorkBenches)
//                .Register();
//        }
//    }

//    class BramblesproutDecoy : DecoyNPC
//    {
//        public override void SetStats()
//        {
//            NPC.width = 18;
//            NPC.height = 36;
//            NPC.lifeMax = 50;
//            NPC.DeathSound = SoundID.NPCDeath15;
//            NPC.noGravity = false;
//            NPC.noTileCollide = false;
//        }
//    }
//}
