using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using System.Collections.Generic;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    abstract class Zweihander : ModItem
	{
		public const int perfectChargeLeeway = 8;

		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Perfectly timed strikes guarentee critical hits");
			Item.ResearchUnlockCount = 1;
		}

		/// <summary>
		/// Use this instead of SetDefaults() for Zweihanders.
		/// </summary>
		public virtual void ZweihanderDefaults()
		{
		}

		/// <summary>
		/// DON'T USE THIS FOR ZWEIHANDERS!!! Use ZweihanderDefaults() instead.
		/// </summary>
        public override void SetDefaults()
		{
			Item.noMelee = false;
			Item.channel = true;
			Item.useStyle = -1;
			Item.shoot = 1;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.autoReuse = true;
			SoundStyle useSound = new SoundStyle("Wisplantern/Sounds/Effects/SwordUnsheath");
			useSound.Pitch -= 0.25f;
			useSound.Volume *= 0.65f;
			useSound.PitchVariance = 0.15f;
			Item.UseSound = useSound;
			ZweihanderDefaults();
		}

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.CopperBar, 12)
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.Wood, 5)
				.AddTile(TileID.Anvils)
				.Register();
        }

		public virtual void WhileCharging(Player player)
        {

        }

		public virtual void OnSwing(Player player)
        {

        }

		public static double AngleDifference(double angle1, double angle2)
		{
			double diff = (angle2 - angle1 + Math.PI / 2) % Math.PI - Math.PI / 2;
			return diff < -Math.PI / 2 ? diff + Math.PI : diff;
		}

        public override bool? CanMeleeAttackCollideWithNPC(Rectangle meleeAttackHitbox, Player player, NPC target)
        {
            Vector2 closestPointOnHitbox = target.Hitbox.Center.ToVector2();
            while (target.Hitbox.Contains((int)closestPointOnHitbox.X, (int)closestPointOnHitbox.Y))
            {
                closestPointOnHitbox += target.DirectionTo(player.itemLocation);
            }
            return !target.friendly && closestPointOnHitbox.Distance(player.itemLocation) < Item.Size.Length() && AngleDifference(player.DirectionTo(target.Center).ToRotation(), rotation + swordRotationAdd * player.direction) <= MathHelper.ToRadians(15f) && goneYet;
        }

        public virtual int DustType => DustID.Torch;
		public virtual int ChargeTime => 55;
		public float chargeProgress = 0f;

        float rotation = 0f;
		float swordRotationAdd = 0f;
		bool goneYet = false;
		public int perfectChargeTime = 0;
		bool hasHitAlready = false;
		float ogRotation = 0f;
		public virtual bool HasSwungDust => false;
		public virtual int SwungDustType => DustID.Torch;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			rotation = velocity.ToRotation();
			swordRotationAdd = -MathHelper.Pi / 2f;
			chargeProgress = 0f;
			perfectChargeTime = 0;
			goneYet = false;
			hasHitAlready = false;
			return false;
        }

        public override void UseAnimation(Player player)
        {

        }

        public override void UseItemFrame(Player player)
		{
			if (goneYet)
			{
				player.compositeFrontArm.stretch = Player.CompositeArmStretchAmount.Full;
				player.compositeFrontArm.enabled = true;
				player.compositeFrontArm.rotation = rotation;
				if (player.direction == 1) player.compositeFrontArm.rotation -= MathHelper.Pi;

				swordRotationAdd += MathHelper.Pi / ((float)Item.useAnimation / player.GetWeaponAttackSpeed(Item));
				rotation += MathHelper.Pi / ((float)Item.useAnimation / player.GetWeaponAttackSpeed(Item)) * player.direction;

				player.itemRotation = rotation + swordRotationAdd * player.direction + MathHelper.ToRadians(player.direction == 1 ? -45 : -135);
				player.itemLocation = player.Center + rotation.ToRotationVector2().RotatedBy(-90 * player.direction) * 12.5f + new Vector2(10, 0).RotatedBy(rotation);

				if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f && HasSwungDust)
				{
					for (float i = 0f; i <= 1f; i += 0.25f)
					{
						float dRotation = rotation + swordRotationAdd * player.direction + MathHelper.Pi * 0.5f * player.direction + MathHelper.Pi * 0.75f - (MathHelper.Pi / ((float)Item.useAnimation / player.GetWeaponAttackSpeed(Item)) * player.direction) * i;
						Vector2 dustPos = player.itemLocation + Item.Size.RotatedBy(dRotation);
						int num3 = Dust.NewDust(dustPos - new Vector2(5, 5), 10, 10, SwungDustType, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
						Main.dust[num3].noLight = true;
						Main.dust[num3].noGravity = true;
						Dust obj = Main.dust[num3];
						obj.velocity *= 0.5f;
					}

				}

				if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f && player.GetModPlayer<Equipable.Accessories.FlintPlayer>().equipped)
                {
					for (float i = 0f; i <= 1f; i += 0.25f)
					{
						float dRotation = rotation + swordRotationAdd * player.direction + MathHelper.Pi * 0.5f * player.direction + MathHelper.Pi * 0.75f - (MathHelper.Pi / ((float)Item.useAnimation / player.GetWeaponAttackSpeed(Item)) * player.direction) * i;
						Vector2 dustPos = player.itemLocation + Item.Size.RotatedBy(dRotation);
						int num3 = Dust.NewDust(dustPos - new Vector2(5, 5), 10, 10, DustID.Torch, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
						Main.dust[num3].noGravity = true;
						Dust obj = Main.dust[num3];
						obj.velocity *= 0.5f;
						obj.velocity += new Vector2(2.5f, 2.5f).RotatedBy(dRotation + Math.PI / 2f * player.direction);
						obj.scale *= 0.75f;
					}
				}
			}
            else
			{
				player.bodyFrame.Y = player.bodyFrame.Height;

				player.itemAnimation = player.itemAnimationMax;
				player.itemTime = player.itemTimeMax;

				player.compositeFrontArm.stretch = Player.CompositeArmStretchAmount.Full;
				player.compositeFrontArm.enabled = true;
				player.compositeFrontArm.rotation = rotation;
				if (player.direction == 1) player.compositeFrontArm.rotation -= MathHelper.Pi;

				player.itemRotation = rotation + swordRotationAdd * player.direction + MathHelper.ToRadians(player.direction == 1 ? -45 : -135);
				player.itemLocation = player.Center + rotation.ToRotationVector2().RotatedBy(-90 * player.direction) * 12.5f + new Vector2(10, 0).RotatedBy(rotation);

				if (chargeProgress < 1f)
				{
					chargeProgress += 1f / ChargeTime;
					if (chargeProgress >= 1f)
					{
						player.DoManaRechargeEffect();
					}
				}
                else
                {
					perfectChargeTime++;
                }

				if (player.channel)
                {
					WhileCharging(player);
				}

				if (!player.channel && chargeProgress >= 0.15f)
				{
					goneYet = true;
					if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f)
					{
						SoundEngine.PlaySound(SoundID.Item117, player.Center);
					}
					player.velocity += rotation.ToRotationVector2() * Item.shootSpeed * chargeProgress * 0.85f;
					ogRotation = rotation;
					OnSwing(player);
					SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, player.Center);
				}
				else if (chargeProgress >= 0.15f && perfectChargeTime <= perfectChargeLeeway)
				{
					Vector2 dustPos = player.itemLocation + Item.Size.RotatedBy(rotation + MathHelper.Pi * 0.75f) * chargeProgress;
					int num3 = Dust.NewDust(dustPos - new Vector2(5, 5), 10, 10, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
					Main.dust[num3].noLight = true;
					Main.dust[num3].noLightEmittence = true;
					Main.dust[num3].noGravity = true;
					Dust obj = Main.dust[num3];
					obj.velocity *= 0.5f;
					obj.velocity.Y -= 1;
					if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f) obj.scale *= 1.5f;
				}
			}

			player.compositeBackArm = player.compositeFrontArm;
			player.compositeBackArm.rotation += swordRotationAdd * player.direction / 2f;
		}

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.SourceDamage.Base *= MathHelper.Lerp(chargeProgress, 1f, 0.2f);
			modifiers.Knockback *= chargeProgress + 0.2f;
			if (!hasHitAlready)
			{
				Vector2 velocityChange = ogRotation.ToRotationVector2() * Item.shootSpeed * chargeProgress;
				velocityChange.Y *= 2f;
				if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f)
				{
					velocityChange.Y *= 1.25f;
					velocityChange.Y += 2.5f;
					SoundStyle style = new SoundStyle("Wisplantern/Sounds/Effects/HeavyMetal");
					style.Volume *= 0.75f;
					style.PitchVariance = 0.35f;
					SoundEngine.PlaySound(style, target.Center);
					//Wisplantern.freezeFrames = 2;
					PunchCameraModifier modifier = new PunchCameraModifier(target.Center, player.velocity.ToRotation().ToRotationVector2().RotatedBy(swordRotationAdd * player.direction + Math.PI / 2), 15f, 10f, 8, 1000f);
					Main.instance.CameraModifiers.Add(modifier);
				}
				player.velocity -= velocityChange;
			}
			if (player.GetModPlayer<Equipable.Accessories.FlintPlayer>().equipped && perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f)
			{
				target.AddBuff(BuffID.OnFire, 180);
			}
			ModifyHitNPCZweihanderVersion(player, target, perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f, !hasHitAlready, ref modifiers);
			hasHitAlready = true;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			int index = tooltips.FindIndex(x => x.Name == "Speed");
			if (index != -1)
            {
				tooltips.RemoveAt(index);

				int speedUsed = (Item.useTime + ChargeTime) / 2;
				string speedDesc = "Insanely fast";
				if (speedUsed > 8) speedDesc = "Very fast";
				if (speedUsed > 20) speedDesc = "Fast";
				if (speedUsed > 25) speedDesc = "Average";
				if (speedUsed > 30) speedDesc = "Slow";
				if (speedUsed > 35) speedDesc = "Very slow";
				if (speedUsed > 45) speedDesc = "Extremely slow";
				if (speedUsed >= 56) speedDesc = "Snail";

				tooltips.Insert(index, new TooltipLine(Mod, "ZweihanderSpeed", speedDesc + " speed"));
            }
        }

        public virtual void ModifyHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, ref NPC.HitModifiers modifiers)
        {

        }
    }
}
