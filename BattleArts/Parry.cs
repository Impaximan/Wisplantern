﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using Terraria.Graphics.CameraModifiers;
using Terraria.Audio;

namespace Wisplantern.BattleArts
{
    //TODO: Make this more risky to pull off
    class Parry : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.SwordParry>();

        public override int ID => BattleArtID.Parry;

        public override string BattleArtDescription => "Right click right before an attack hits you to negate 80% of the damage and knock nearby enemies away" +
            "\n8 second cooldown, but only if you miss the parry";

        public override string BattleArtName => "Sword Parry";

        public override BattleArtType BattleArtType => BattleArtType.Sword;

        public override Color Color => Color.SkyBlue;

        float rotationAddend = 0f;
        public override void PreUseBattleArt(ref Item item, Player player)
        {
            item.useTime = 10;
            item.useAnimation = 10;
            item.shoot = ProjectileID.None;
            item.knockBack = 0;
            item.UseSound = new SoundStyle("Wisplantern/Sounds/Effects/SwordUnsheath");
            rotationAddend = 0f;
            player.GetModPlayer<ParryPlayer>().parryTime = item.useTime;
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), 60 * 8);
        }

        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            player.itemRotation = MathHelper.ToRadians(90 + rotationAddend) * player.direction;
            rotationAddend -= 4f;
            player.itemLocation = player.Center + new Vector2(0, -16);
        }

        public override void UseBattleArt(Item item, Player player, bool firstFrame)
        {

        }

        public override void PostUpdatePlayer(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 2;
        }
    }

    class ParryPlayer : ModPlayer
    {
        public int parryTime;

        public void Parry()
        {
            Wisplantern.freezeFrameLight = true;
            Wisplantern.freezeFrames = 5;
            Player.immuneTime = 60;

            PunchCameraModifier modifier = new PunchCameraModifier(Player.Center, new Vector2(Player.direction, 0), 15f, 10f, 8, 1000f);
            Main.instance.CameraModifiers.Add(modifier);

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.CountsAsACritter && !npc.friendly && npc.Distance(Player.Center) <= Player.HeldItem.Size.Length() * 3f)
                {
                    npc.StrikeNPC(Player.HeldItem.damage * 3, 8f, (int)(npc.Center.X - Player.Center.X), true, false, false);
                    npc.netUpdate = true;
                }
            }
            SoundEngine.PlaySound(new SoundStyle("Wisplantern/Sounds/Effects/HeavyMetal"), Player.Center);
            Player.DoManaRechargeEffect();
            Player.ClearBuff(ModContent.BuffType<Buffs.BattleArtCooldown>());
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (parryTime > 0)
            {
                Parry();
                damage /= 5;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (parryTime > 0)
            {
                Parry();
                damage /= 5;
            }
        }

        public override void PostUpdate()
        {
            if (parryTime > 0)
            {
                parryTime--;
            }
        }
    }
}