using Terraria.GameContent.Creative;
using Wisplantern.Buffs;
using static Wisplantern.Globals.GNPCs.InfightingNPC;

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
            Item.SetManipulativePower(0.3f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.SetScholarlyDescription("Found mysteriously from the mystical remains of Wisplanterns underground");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override float MaxDistance => 400f;

        public override int DustType => ModContent.DustType<Dusts.HyperstoneDustFast>();

        public override void OnAggravate(NPC npc, Player player)
        {
            npc.AddBuff(ModContent.BuffType<Vulnerable>(), GetAggravationTime(npc));
        }
    }
}
