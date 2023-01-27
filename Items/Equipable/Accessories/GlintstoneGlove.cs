using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Wisplantern.Items.Weapons.Magic.Staffs;

namespace Wisplantern.Items.Equipable.Accessories
{
    class GlintstoneGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows you to swing magic staffs, recovering mana on melee hits" +
                "\n+20 max mana");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<GlintstoneGlovePlayer>().equipped = true;
            player.statManaMax2 += 20;
        }
    }

    class GlintstoneGlovePlayer : ModPlayer
    {
        public bool equipped = false;

        public override void ResetEffects()
        {
            equipped = false;
        }
    }

    class GlintstoneGloveItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.GetModPlayer<GlintstoneGlovePlayer>().equipped && Item.staff[item.type])
            {
                position += new Vector2(item.width * -player.direction, -item.height);
                if (player.whoAmI == Main.myPlayer && item.type != ModContent.ItemType<Plantscalibur>()) velocity = (Main.MouseWorld - position).ToRotation().ToRotationVector2() * velocity.Length();
            }
        }

        int originalItemUseStyle;
        bool originallyNoMelee;
        public override void SetDefaults(Item item)
        {
            originalItemUseStyle = item.useStyle;
            originallyNoMelee = item.noMelee;
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (player.GetModPlayer<GlintstoneGlovePlayer>().equipped && Item.staff[item.type])
            {
                knockBack *= 1.5f;  
                damage = (int)(damage * 0.85f);
            }
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (player.GetModPlayer<GlintstoneGlovePlayer>().equipped && Item.staff[item.type])
            {
                int amount = (int)(item.mana * 1.65f);
                player.ManaEffect(amount);
                player.statMana += amount;
            }
        }

        public override void HoldItem(Item item, Player player)
        {

            if (player.GetModPlayer<GlintstoneGlovePlayer>().equipped && Item.staff[item.type])
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
