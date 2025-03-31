using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class GuidingLight : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 15;
            Item.SetManipulativePower(0.25f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 0, 25);
            Item.SetScholarlyDescription("Found mysteriously from the mystical remains of Wisplanterns underground");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override float MaxDistance => 300f;

        public override int DustType => ModContent.DustType<Dusts.HyperstoneDustFast>();

        public override void OnAggravate(NPC npc, Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.Hyperspeed>(), 60 * 2);
        }
    }
}
