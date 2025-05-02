using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class BloodyRoots : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Stinger, 12)
                .AddIngredient(ItemID.JungleSpores, 15)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.BladeofGrass);
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 24;
            Item.SetManipulativePower(0.26f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 52;
            Item.height = 46;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 0, 25);
        }

        public override float MaxDistance => 380f;

        public override int DustType => DustID.JungleGrass;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 60 * 5);
        }
    }
}
