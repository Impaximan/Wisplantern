using Terraria.DataStructures;
using System;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using System.Collections.Generic;
using System.IO;

namespace Wisplantern.Items.Weapons.Melee.Zweihanders
{
    public abstract class Zweihander : ModItem
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
			SoundStyle useSound = new("Wisplantern/Sounds/Effects/DrawSword");
			//useSound.Pitch -= 0.25f;
			useSound.Volume *= 0.1f;
			useSound.PitchVariance = 0.15f;
			Item.UseSound = useSound;
			ZweihanderDefaults();
		}

		public virtual void WhileCharging(Player player)
        {

        }

		public virtual void OnSwing(Player player, bool perfectCharge)
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
            return closestPointOnHitbox.Distance(player.itemLocation) < Item.Size.Length() && Math.Abs(AngleDifference(player.DirectionTo(target.Center).ToRotation(), rotation + swordRotationAdd * player.direction)) <= MathHelper.ToRadians(15f) && goneYet;
        }

        public virtual int DustType => DustID.Torch;
		public virtual int ChargeTime => 55;
		public float chargeProgress = 0f;

        public float rotation = 0f;
		public float swordRotationAdd = 0f;
		public bool goneYet = false;
		public int perfectChargeTime = 0;
		public bool hasHitAlready = false;
		public bool hasHitAlready2 = false;
		public float ogRotation = 0f;
		public int freezeFrames = 0;
		public virtual bool HasSwungDust => false;
		public virtual int SwungDustType => DustID.Torch;

		public virtual float SwungDustSize => 1f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			rotation = velocity.ToRotation();
			swordRotationAdd = -MathHelper.Pi / 2f;
			chargeProgress = 0f;
			perfectChargeTime = 0;
			goneYet = false;
			hasHitAlready = false;
			hasHitAlready2 = false;
			return false;
        }

        public override void UseAnimation(Player player)
        {

        }

        public override void UseItemFrame(Player player)
		{
			bool useZweihander = false;

			if (goneYet)
			{
				player.compositeFrontArm.stretch = Player.CompositeArmStretchAmount.Full;
				player.compositeFrontArm.enabled = true;
				player.compositeFrontArm.rotation = rotation;
				if (player.direction == 1) player.compositeFrontArm.rotation -= MathHelper.Pi;

				if (freezeFrames <= 0)
                {
                    swordRotationAdd += MathHelper.Pi / ((float)Item.useAnimation / player.GetWeaponAttackSpeed(Item));
                    rotation += MathHelper.Pi / ((float)Item.useAnimation / player.GetWeaponAttackSpeed(Item)) * player.direction;
                }
				else
				{
					freezeFrames--;
					player.itemTime++;
					player.itemAnimation++;

                }

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
						obj.scale *= SwungDustSize;
					}

				}

				if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f && player.AccessoryActive<Equipable.Accessories.Flint>())
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
					chargeProgress += 1f / ChargeTime * player.GetModPlayer<EquipmentPlayer>().zweihanderSpeed;
					if (chargeProgress >= 1f && player.whoAmI == Main.myPlayer)
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
					useZweihander = true;
					if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f)
                    {
                        SoundStyle slash = new("Wisplantern/Sounds/Effects/ZweihanderWoosh");
                        slash.Volume *= 0.4f;
						slash.Pitch -= 0.5f;
                        slash.PitchVariance = 0.3f;
                        SoundEngine.PlaySound(slash, player.Center);
                    }
					player.velocity += rotation.ToRotationVector2() * Item.shootSpeed * chargeProgress * 0.85f;
					ogRotation = rotation;
					OnSwing(player, perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f);
                    SoundStyle useSound = new("Wisplantern/Sounds/Effects/SwordSlash");
                    useSound.Volume *= 0.5f;
                    useSound.PitchVariance = 0.25f;
                    SoundEngine.PlaySound(useSound, player.Center);
					if (Main.netMode != NetmodeID.SinglePlayer && player.whoAmI == Main.myPlayer)
					{
						NetMessage.SendData(MessageID.SyncPlayer, number: player.whoAmI);
					}
				}
				else if (chargeProgress >= 0.15f && perfectChargeTime <= perfectChargeLeeway && player.whoAmI == Main.myPlayer)
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

			if (Main.netMode != NetmodeID.SinglePlayer && player.whoAmI == Main.myPlayer)
			{
				Mod.SendPacket(new ZweihanderSync(player.whoAmI, chargeProgress, rotation, swordRotationAdd, goneYet, perfectChargeTime, hasHitAlready, hasHitAlready2, ogRotation, useZweihander, player.velocity.X, player.velocity.Y, player.direction, freezeFrames), -1, Main.myPlayer, true);
			}
		}

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= MathHelper.Lerp(chargeProgress, 1f, 0.2f);
			modifiers.Knockback *= chargeProgress + 0.2f;
			if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f)
			{
				modifiers.SourceDamage *= 1.3f;
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

        public sealed override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (!hasHitAlready2)
			{
				Vector2 velocityChange = ogRotation.ToRotationVector2() * Item.shootSpeed * chargeProgress;
				velocityChange.Y *= 2f;
				if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f)
				{
					velocityChange.Y *= 1.25f;
					velocityChange.Y += 2.5f;
					SoundStyle style = new("Wisplantern/Sounds/Effects/ZweihanderHit");
					style.Volume *= 0.6f;
					style.Pitch -= 0.25f;
					style.PitchVariance = 0.3f;
					SoundEngine.PlaySound(style, target.Center);
					PunchCameraModifier modifier = new(target.Center, player.velocity.ToRotation().ToRotationVector2().RotatedBy(swordRotationAdd * player.direction + Math.PI / 2), 15f, 10f, 8, 1000f);
					Main.instance.CameraModifiers.Add(modifier);
                    freezeFrames = 5;
                }
				player.velocity -= velocityChange;
                if (Main.netMode != NetmodeID.SinglePlayer && player.whoAmI == Main.myPlayer)
				{
					Mod.SendPacket(new SyncPlayerVelocity(player.velocity.X, player.velocity.Y, player.whoAmI), -1, player.whoAmI, true);
				}
			}
			if (perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f)
            {
                target.immune[player.whoAmI] = Item.useAnimation + freezeFrames;
            }
			if (player.AccessoryActive<Equipable.Accessories.Flint>() && perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f)
			{
				target.AddBuff(BuffID.OnFire, 180);
			}
			OnHitNPCZweihanderVersion(player, target, perfectChargeTime <= perfectChargeLeeway && chargeProgress >= 1f, !hasHitAlready2, hit, damageDone);
			hasHitAlready2 = true;
        }

		public virtual void OnHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, NPC.HitInfo hit, int damageDone)
		{
		}

		public virtual void ModifyHitNPCZweihanderVersion(Player player, NPC target, bool perfectCharge, bool firstHit, ref NPC.HitModifiers modifiers)
        {

        }
    }

    public readonly struct ZweihanderSync : IEasyPacket<ZweihanderSync>
    {
		public readonly int player;
        public readonly float chargeProgress;
        public readonly float rotation;
        public readonly float swordRotationAdd;
        public readonly bool goneYet;
        public readonly int perfectChargeTime;
        public readonly bool hasHitAlready;
        public readonly bool hasHitAlready2;
        public readonly float ogRotation;
		public readonly bool useZweihander;
		public readonly float vX;
		public readonly float vY;
		public readonly int direction;
		public readonly int freezeFrames;

		public ZweihanderSync(int player, float chargeProgress, float rotation, float swordRotationAdd, bool goneYet, int perfectChargeTime, bool hasHitAlready, bool hasHitAlready2, float ogRotation, bool useZweihander, float vX, float vY, int direction, int freezeFrames)
		{
			this.player = player;
			this.chargeProgress = chargeProgress;
			this.rotation = rotation;
			this.swordRotationAdd = swordRotationAdd;
			this.goneYet = goneYet;
			this.perfectChargeTime = perfectChargeTime;
			this.hasHitAlready = hasHitAlready;
			this.hasHitAlready2 = hasHitAlready2;
			this.ogRotation = ogRotation;
			this.useZweihander = useZweihander;
			this.vX = vX;
			this.vY = vY;
			this.direction = direction;
            this.freezeFrames = freezeFrames;
        }

        public ZweihanderSync Deserialise(BinaryReader reader, in SenderInfo sender)
        {
			return new ZweihanderSync(reader.ReadInt32(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadBoolean(), reader.ReadInt32(), reader.ReadBoolean(), reader.ReadBoolean(), reader.ReadSingle(), reader.ReadBoolean(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadInt32(), reader.ReadInt32());
        }

        public void Serialise(BinaryWriter writer)
        {
			writer.Write(player);
			writer.Write(chargeProgress);
			writer.Write(rotation);
			writer.Write(swordRotationAdd);
			writer.Write(goneYet);
			writer.Write(perfectChargeTime);
			writer.Write(hasHitAlready);
			writer.Write(hasHitAlready2);
			writer.Write(ogRotation);
			writer.Write(useZweihander);
			writer.Write(vX);
			writer.Write(vY);
			writer.Write(direction);
			writer.Write(freezeFrames);
        }
    }

    public readonly struct ZweihanderSyncHandler : IEasyPacketHandler<ZweihanderSync>
    {
        void IEasyPacketHandler<ZweihanderSync>.Receive(in ZweihanderSync packet, in SenderInfo sender, ref bool handled)
        {
            Player player = Main.player[packet.player];
			if (player.HeldItem.ModItem is Zweihander zweihander)
			{
				zweihander.chargeProgress = packet.chargeProgress;
				zweihander.rotation = packet.rotation;
				zweihander.swordRotationAdd = packet.swordRotationAdd;
				zweihander.goneYet = packet.goneYet;
				zweihander.perfectChargeTime = packet.perfectChargeTime;
				zweihander.hasHitAlready = packet.hasHitAlready;
				zweihander.hasHitAlready2 = packet.hasHitAlready2;
				zweihander.ogRotation = packet.ogRotation;
				zweihander.freezeFrames = packet.freezeFrames;
				player.direction = packet.direction;

                if (packet.useZweihander)
                {
                    if (zweihander.perfectChargeTime <= Zweihander.perfectChargeLeeway && zweihander.chargeProgress >= 1f)
                    {
                        SoundStyle slash = new("Wisplantern/Sounds/Effects/ZweihanderWoosh");
                        slash.Volume *= 0.4f;
                        slash.Pitch -= 0.5f;
                        slash.PitchVariance = 0.5f;
                        SoundEngine.PlaySound(slash, player.Center);
                    }
                    SoundStyle useSound = new("Wisplantern/Sounds/Effects/SwordSlash");
                    useSound.Volume *= 0.5f;
                    useSound.PitchVariance = 0.25f;
                    SoundEngine.PlaySound(useSound, player.Center);
                    player.velocity = new Vector2(packet.vX, packet.vY);
                }
			}

            handled = true;
        }
    }
}
