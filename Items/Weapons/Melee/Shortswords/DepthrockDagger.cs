using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.Graphics.CameraModifiers;
using Terraria.Audio;

namespace Wisplantern.Items.Weapons.Melee.Shortswords
{
	//TODO: Add an indicator for when the fourth hit is charged up and for when you lose the jab counter
    class DepthrockDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Every fourth hit will jab a dagger into the target");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.knockBack = 3f;
            Item.shoot = ModContent.ProjectileType<DepthrockDaggerProjectile>();
            Item.shootSpeed = 2.5f;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.width = 20;
            Item.height = 26;
			Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 10, 0);
        }

		int inventoryTime = 0;
		int cooldown = 0;
        public override void UpdateInventory(Player player)
        {
			inventoryTime++;
			if (inventoryTime > 60)
            {
				totalShots = 0;
            }

			if (cooldown > 0)
            {
				cooldown--;
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			if (totalShots >= 3)
            {
				velocity *= 2.3f;
				damage = (int)(damage * 1.5f);
				knockback *= 3f;
            }
        }

        public override bool CanUseItem(Player player)
        {
			if (cooldown > 0)
            {
				return false;
            }
            return base.CanUseItem(player);
        }

        int totalShots = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			inventoryTime = 0;
			if (totalShots >= 3)
            {
				totalShots = 0;
				Item.stack--;
				if (Main.netMode != NetmodeID.MultiplayerClient) Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 1);
				SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, player.Center);
				cooldown = 20;
				return false;
            }
            else
			{
				totalShots++;
			}
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient<Placeable.Blocks.Depthrock>(30)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

	public class DepthrockDaggerProjectile : ModProjectile
	{
        public override string Texture => "Wisplantern/Items/Weapons/Melee/Shortswords/DepthrockDagger";

        public const int FadeInDuration = 7;
		public const int FadeOutDuration = 4;

		public const int TotalDuration = 16;
		public float CollisionWidth => 10f * Projectile.scale;

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depthrock Dagger");
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(18);
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1.3f;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 360;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (stuckNPC == null && Projectile.ai[1] == 1)
            {
				stuckNPC = target;
				stuckOffset = (Projectile.position + Projectile.velocity * 2f) - target.position;

				PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, Projectile.velocity.ToRotation().ToRotationVector2(), 20f, 10f, 10, 1000f);
				Main.instance.CameraModifiers.Add(modifier);

				SoundStyle style = SoundID.DD2_MonkStaffGroundImpact;
				style.Volume *= 1.5f;
				SoundEngine.PlaySound(style, target.Center);

				Wisplantern.freezeFrames = 5;

				Projectile.localNPCHitCooldown = 10;
				Projectile.ownerHitCheck = false;
			}
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (stuckNPC != null && stuckNPC == target)
            {
				knockback = 0f;
				damage = (int)(damage * 0.35f) + (int)MathHelper.Clamp(target.defense - 10, 0, target.defense) / 2;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
			if (stuckNPC == null)
            {
				return null;
            }
            return stuckNPC == target;
        }

        NPC stuckNPC = null;
		Vector2 stuckOffset = Vector2.Zero;
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (stuckNPC == null)
			{
				Timer += 1;
				if (Timer >= TotalDuration)
				{
					Projectile.Kill();
					return;
				}
				else
				{
					player.heldProj = Projectile.whoAmI;
				}

				Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

				Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
				Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

				Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();

				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
			}
            else
            {
				Projectile.position = stuckNPC.position + stuckOffset;
				Projectile.Opacity = 1f;
				Projectile.hide = false;
				if (stuckNPC.active == false)
                {
					Projectile.timeLeft = 0;
                }
			}
			SetVisualOffsets();
		}

		private void SetVisualOffsets()
		{
			const int HalfSpriteWidth = 20 / 2;
			const int HalfSpriteHeight = 26 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
			Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (stuckNPC != null)
			{
				Vector2 start1 = Projectile.Center;
				Vector2 end1 = start1 + Projectile.velocity * 20f;
				float collisionPoint1 = 0f;
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start1, end1, CollisionWidth * 2f, ref collisionPoint1);
			}
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity * 6f;
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
		}
	}
}
