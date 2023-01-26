using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.Graphics.CameraModifiers;
using System;
using Terraria.Audio;

namespace Wisplantern.Items.Weapons.Melee.Swords
{
    class WeatheredSledgehammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Guarenteed critical strike on uninjured enemies" +
                "\nCan be upgraded with better stone");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.damage = 17;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.UseSound = SoundID.DD2_MonkStaffSwing;
            Item.knockBack = 6.5f;
            Item.scale = 1.25f;
            Item.crit = 6;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Melee;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (target.life == target.lifeMax)
            {
                crit = true;
                Wisplantern.freezeFrames = 5;

                PunchCameraModifier modifier = new PunchCameraModifier(target.Center, new Vector2(Math.Sign(target.Center.X - player.Center.X), 0), 20f, 10f, 10, 1000f);
                Main.instance.CameraModifiers.Add(modifier);

                SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/HeavyHit")
                {
                    MaxInstances = 0,
                    PitchVariance = 0.5f
                };
                SoundEngine.PlaySound(style, target.Center);
            }
        }
    }
}
