using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class WoodenCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 11)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = ModContent.GetInstance<DamageClasses.Manipulative>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = 1;
            Item.width = 30;
            Item.height = 42;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shootSpeed = 1f;
            Item.value = Item.sellPrice(0, 0, 0, 25);
            Item.SetManipulativePower(0.08f);
        }

        const float maxDistance = 225f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(player.Center) <= maxDistance)
                {
                    Item.AggravateNPC(npc, player);
                }
            }
            for (int i = 0; i < 150; i++)
            {
                Vector2 dustPos = Main.rand.NextVector2CircularEdge(maxDistance, maxDistance) + player.Center;
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.WoodFurniture, player.velocity);
                dust.noGravity = true;
                dust.noLight = true;
            }
            return false;
        }
    }

}
