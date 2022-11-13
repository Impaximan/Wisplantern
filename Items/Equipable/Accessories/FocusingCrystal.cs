using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;

namespace Wisplantern.Items.Equipable.Accessories
{
    class FocusingCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Critical strikes are more deadly" +
                "\n5% increased critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<FocusingCrystalPlayer>().equipped = true;
            player.GetCritChance(DamageClass.Generic) += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Items.Placeable.Blocks.Fulgarite>(25)
                .AddIngredient(ItemID.Sapphire, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class FocusingCrystalPlayer : ModPlayer
    {
        public bool equipped = false;

        public override void ResetEffects()
        {
            equipped = false;
        }

        const float critDamageMult = 1.3f;
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (equipped && crit)
            {
                damage = (int)(damage * critDamageMult);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (equipped && crit)
            {
                damage = (int)(damage * critDamageMult);
            }
        }
    }
}
