using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Wisplantern.Buffs;
using Wisplantern.Items.Materials;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    public class FocusedMayhem : Zweihander
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void ZweihanderDefaults()
        {
            Item.knockBack = 5f;
            Item.width = 72;
            Item.height = 64;
            Item.damage = 64;
            Item.shootSpeed = 9f;
            Item.rare = ItemRarityID.Orange;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 0, 54, 0);
        }

        public override bool HasSwungDust => true;

        public override int SwungDustType => DustID.GreenTorch;

        public override float SwungDustSize => 1f;

        public override void ModifyHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= damageMult;
        }

        public override void OnHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, NPC.HitInfo hit, int damageDone)
        {
            if (perfectCharge)
            {
                CombatText.NewText(player.getRect(), Color.LimeGreen, Math.Round(damageMult, 1).ToString() + "x", true);
                damageMult += 0.2f;
                if (damageMult > 4f)
                {
                    damageMult = 4f;
                }
                SoundStyle style = SoundID.Item35;
                style.PitchVariance = 0f;
                style.Volume *= 2f;
                style.Pitch = Math.Clamp(-0.5f + (damageMult - 1f) / 12f * 5f, -1f, 1f);
                SoundEngine.PlaySound(style, player.Center);
            }
            else if (damageMult > 1f)
            {
                CombatText.NewText(player.getRect(), Color.Red, Math.Round(damageMult, 1).ToString() + "x", true);
                damageMult = 1f;
                SoundEngine.PlaySound(SoundID.Item16, player.Center);
            }
        }

        public float damageMult = 1f;

        public float speedMult = 1f;

        public override int ChargeTime => (int)(base.ChargeTime * speedMult);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            speedMult = Main.rand.NextFloat(0.7f, 1.3f);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1f - (Item.alpha / 255f));
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