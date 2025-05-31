using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    public class HuntressSickle : Zweihander
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Perfectly timed strikes guarentee critical hits");
            Item.ResearchUnlockCount = 1;
        }

        public override void ZweihanderDefaults()
        {
            Item.knockBack = 9f;
            Item.width = 62;
            Item.height = 64;
            Item.damage = 30;
            Item.shootSpeed = 8.5f;
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = DamageClass.Melee;
            Item.MarkAsHuntingWeapon();
            Item.value = Item.buyPrice(0, 7, 50, 0);
        }

        public override void ModifyHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, ref NPC.HitModifiers modifiers)
        {
            //if (player.GetModPlayer<ManipulativePlayer>().smokeBombTime > 0)
            //{
            //    modifiers.SetCrit();
            //}
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff<SickleSmokeBombCooldown>())
            {
                return base.Shoot(player, source, position, velocity, type, damage, knockback);
            }

            player.SmokeBomb((int)(ChargeTime / player.GetModPlayer<EquipmentPlayer>().zweihanderSpeed) + 5);
            player.AddBuff(ModContent.BuffType<SickleSmokeBombCooldown>(), 60 * 5);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void OnSwing(Player player, bool perfectCharge)
        {
            if (player.GetModPlayer<ManipulativePlayer>().smokeBombTime > 0)
            {
                player.GetModPlayer<ManipulativePlayer>().smokeBombTime = 1;
                player.immune = true;
                player.immuneTime = 10;
            }
        }

        public override void OnHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, NPC.HitInfo hit, int damageDone)
        {
            //if (player.GetModPlayer<ManipulativePlayer>().smokeBombTime > 0)
            //{

            //}
            //else if (perfectCharge)
            //{
            //    player.SmokeBomb(60 * 3);
            //}
        }

    }

    public class SickleSmokeBombCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.buffTime[buffIndex] == 1)
                {
                    player.DoManaRechargeEffect();
                }
            }
        }
    }
}
