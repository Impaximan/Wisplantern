using Wisplantern.Buffs;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    internal class GrassCutter : Zweihander
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Perfectly timed strikes guarentee critical hits");
            Item.ResearchUnlockCount = 1;
        }

        public override void ZweihanderDefaults()
        {
            Item.knockBack = 5f;
            Item.width = 62;
            Item.height = 68;
            Item.damage = 37;
            Item.shootSpeed = 8.5f;
            Item.rare = ItemRarityID.Orange;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 0, 65, 0);
        }

        public override bool HasSwungDust => true;

        public override int SwungDustType => DustID.JungleSpore;

        public override float SwungDustSize => 0.5f;

        public override void OnHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, NPC.HitInfo hit, int damageDone)
        {
            if (perfectCharge)
            {
                target.AddBuff(BuffID.Poisoned, 60 * 10);
                player.AddBuff(ModContent.BuffType<GrassCutterBuff>(), 60 * 4);
                player.Heal(5);

            }
            else
            {
                target.AddBuff(BuffID.Poisoned, 60);
            }
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Stinger, 13)
                .AddIngredient(ItemID.JungleSpores, 16)
                .AddIngredient(ItemID.RichMahogany, 5)
                .AddTile(TileID.Anvils)
                .Register()
                .SortAfterFirstRecipesOf(ItemID.BladeofGrass);
        }
    }
}
