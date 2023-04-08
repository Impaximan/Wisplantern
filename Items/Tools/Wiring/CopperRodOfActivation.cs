using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using System.Reflection;

namespace Wisplantern.Items.Tools.Wiring
{
    class CopperRodOfActivation : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.DD2_DefenseTowerSpawn;
            Item.rare = ItemRarityID.White;
            Item.mech = true;
            Item.autoReuse = true;
            Item.tileBoost = 20;
            Item.useTurn = true;
            Item.SetScholarlyDescription("Hint hint: the statues found underground may have more to them than you think");
        }

        public override bool CanUseItem(Player player)
        {
            int i = (int)Main.MouseWorld.X / 16;
            int j = (int)Main.MouseWorld.Y / 16;
            int range = player.blockRange + Item.tileBoost;
            Point16 point = player.Center.ToTileCoordinates16();
            if (i > point.X - range && i < point.X + range && j > point.Y - range && j < point.Y + range)
            {
                typeof(Terraria.Wiring).GetMethod("HitWireSingle", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[]
                {
                    i,
                    j,
                });
                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
