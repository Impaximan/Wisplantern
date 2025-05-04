using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Wisplantern.DamageClasses;
using Wisplantern.Globals.GItems;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Equipable.Armor.Pandemonium
{
    [AutoloadEquip(EquipType.Head)]
    public class PandemoniumHood : ModItem
    {
        public static LocalizedText SetBonus { get; private set; }

        public override void SetStaticDefaults()
        {
            SetBonus = Mod.GetLocalization($"{nameof(PandemoniumHood)}.SetBonus");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
            Item.MarkAsShepherd();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(ModContent.GetInstance<ManipulativeDamageClass>()) += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == Type && body.type == ModContent.ItemType<PandemoniumBreastplate>() && legs.type == ModContent.ItemType<PandemoniumLeggings>();
        }

        public int burstJumpCounter = 0;
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonus.Value;

            player.GetCritChance(ModContent.GetInstance<ManipulativeDamageClass>()) += 5f;

            if (player.controlUp)
            {
                burstJumpCounter++;

                if (burstJumpCounter == 120)
                {
                    SoundEngine.PlaySound(SoundID.Item37, player.Center);
                    CombatText.NewText(player.getRect(), Color.LightGreen, "Burst jump ready...");
                }
            }
            else
            {
                if (burstJumpCounter > 120)
                {
                    SoundEngine.PlaySound(SoundID.Item62, player.Center);
                    CombatText.NewText(player.getRect(), Color.LimeGreen, Main.rand.Next(new List<string>()
                    {
                        "WHEEEE!",
                        "KABOOM!",
                        "KAPOW!",
                        "BOMBS AWAY!",
                        "YIPPEE!"
                    }), true);

                    player.SmokeBomb(30);

                    int direction = 0;

                    if (player.controlLeft) direction--;
                    if (player.controlRight) direction++;

                    if (player.whoAmI == Main.myPlayer) player.velocity = new Vector2(15f * direction, -15f);

                    if (player.whoAmI == Main.myPlayer) Wisplantern.freezeFrameLight = true;
                    Wisplantern.freezeFrames = 10;

                    PunchCameraModifier modifier = new(player.Center, player.velocity.ToRotation().ToRotationVector2(), 10f, 10f, 8, 1000f);
                    Main.instance.CameraModifiers.Add(modifier);

                    if (Main.netMode != NetmodeID.SinglePlayer && player.whoAmI == Main.myPlayer) Mod.SendPacket(new SyncPlayerVelocity(player.velocity.X, player.velocity.Y, player.whoAmI), forward: true);
                }

                burstJumpCounter = 0;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PandemoniumBar>(22)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}