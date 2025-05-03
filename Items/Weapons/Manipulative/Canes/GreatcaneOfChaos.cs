using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Terraria.Audio;
using Wisplantern.Items.Materials;
using Wisplantern.Buffs;
using Wisplantern.Globals.GNPCs;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    class GreatcaneOfChaos : CaneWeapon
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PandemoniumBar>(25)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void CaneSetDefaults()
        {
            Item.damage = 25;
            Item.SetManipulativePower(0.25f);
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            Item.width = 68;
            Item.height = 60;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override float MaxDistance => 425f;

        public override int DustType => DustID.GreenTorch;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override void OnAggravate(NPC npc, Player player)
        {
            npc.AddBuff(ModContent.BuffType<ChaosElement>(), InfightingNPC.GetAggravationTime(npc));
            npc.GetGlobalNPC<InfightingNPC>().chaosTeleportCooldown = Main.rand.Next(60);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
