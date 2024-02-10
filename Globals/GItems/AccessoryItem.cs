using Wisplantern.Items.Equipable.Accessories;

namespace Wisplantern.Globals.GItems
{
    /// <summary>
    /// For changes that accessories make.
    /// </summary>
    public class AccessoryItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.AccessoryActive<DraconicDacron>())
            {
                if (type == ProjectileID.WoodenArrowFriendly && Main.rand.NextBool(6))
                {
                    type = ProjectileID.FireArrow;
                    damage += 2;
                }
            }

            if (player.AccessoryActive<GlintstoneGlove>() && Item.staff[item.type])
            {
                position += new Vector2(item.width * -player.direction, -item.height);
                if (player.whoAmI == Main.myPlayer && item.type != ModContent.ItemType<Items.Weapons.Magic.Staffs.Plantscalibur>() && !player.GetModPlayer<Globals.GItems.BattleArtPlayer>().usingBattleArt) velocity = (Main.MouseWorld - position).ToRotation().ToRotationVector2() * velocity.Length();
            }
        }

        int originalItemUseStyle;
        bool originallyNoMelee;
        public override void SetDefaults(Item item)
        {
            originalItemUseStyle = item.useStyle;
            originallyNoMelee = item.noMelee;
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (player.AccessoryActive<GlintstoneGlove>() && Item.staff[item.type])
            {
                modifiers.Knockback *= 1.5f;
                modifiers.FinalDamage *= 0.85f;
            }
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.AccessoryActive<GlintstoneGlove>() && Item.staff[item.type])
            {
                int amount = (int)(item.mana * 1.65f);
                player.ManaEffect(amount);
                player.statMana += amount;
            }
        }

        public override void HoldItem(Item item, Player player)
        {

            if (player.AccessoryActive<GlintstoneGlove>() && Item.staff[item.type])
            {
                item.useStyle = ItemUseStyleID.Swing;
                item.noMelee = false;
            }
            else
            {
                item.useStyle = originalItemUseStyle;
                item.noMelee = originallyNoMelee;
            }
        }
    }
}
