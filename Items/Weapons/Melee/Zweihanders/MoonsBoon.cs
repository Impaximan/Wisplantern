using System;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    internal class MoonsBoon : Zweihander
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Perfectly timed strikes guarentee critical hits");
            Item.ResearchUnlockCount = 1;
        }

        public override void ZweihanderDefaults()
        {
            Item.knockBack = 9f;
            Item.width = 60;
            Item.height = 66;
            Item.damage = 37;
            Item.shootSpeed = 7f;
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = DamageClass.Melee;
        }

        const float minDistance = 0f;
        const float maxDistance = 300f;
        public override void OnSwing(Player player, bool perfectCharge)
        {
            if (perfectCharge)
            {
                if (Collision.CanHit(player.position, player.width, player.height, Main.MouseWorld - player.Size / 2, player.width, player.height))
                {
                    player.immune = true;
                    player.immuneTime = 30;
                    player.Teleport(player.position + player.DirectionTo(Main.MouseWorld) * Math.Clamp(player.Distance(Main.MouseWorld), minDistance, maxDistance), TeleportationStyleID.RodOfDiscord);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        Mod.SendPacket(new SyncPlayerTeleport(player.position.X, player.position.Y, player.whoAmI, TeleportationStyleID.RodOfDiscord), -1, player.whoAmI, true);
                    }
                }
            }
        }

        public override bool HasSwungDust => true;
        public override int SwungDustType => DustID.Shadowflame;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
