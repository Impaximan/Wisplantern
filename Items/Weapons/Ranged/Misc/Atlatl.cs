using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria;

namespace Wisplantern.Items.Weapons.Ranged.Misc
{
    class Atlatl : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Dart;
            Item.width = 38;
            Item.height = 44;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item17;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.MarkAsHuntingWeapon();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Speed");
            if (index != -1)
            {
                tooltips.RemoveAt(index);

                float speedUsed = Item.useTime / (1f + Main.LocalPlayer.velocity.Length() / 5f);
                string speedDesc = "Insanely fast";
                if (speedUsed > 8) speedDesc = "Very fast";
                if (speedUsed > 20) speedDesc = "Fast";
                if (speedUsed > 25) speedDesc = "Average";
                if (speedUsed > 30) speedDesc = "Slow";
                if (speedUsed > 35) speedDesc = "Very slow";
                if (speedUsed > 45) speedDesc = "Extremely slow";
                if (speedUsed >= 56) speedDesc = "Snail";

                tooltips.Insert(index, new TooltipLine(Mod, "AtlatlSpeed", speedDesc + " speed"));
            }
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return (1f + player.velocity.Length() / 5f);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextBool(7, 10);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.Y -= 25;
            position.X -= 10 * player.direction;

            velocity = position.DirectionTo(Main.MouseWorld) * velocity.Length();
        }
    }
}
