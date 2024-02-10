using System;

namespace Wisplantern.Items.Weapons.Summon.Minions
{
    internal class SilverStaff : MinionStaff
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 8;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.damage = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item44;
            Item.buffType = ModContent.BuffType<SilverSummon>();
            Item.shoot = ModContent.ProjectileType<SilverSummonProjectile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 14)
                .AddIngredient(ItemID.FallenStar, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class SilverSummon : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }

    class SilverSummonProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SilverBroadsword;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        int chargeCounter = 0;
        public override void AI()
        {
            Projectile.timeLeft = 60;

            Player owner = Main.player[Projectile.owner];

            if (owner.dead)
            {
                Projectile.active = false;
            }

            if (Main.myPlayer == owner.whoAmI && Projectile.Distance(owner.Center) >= 2000f)
            {
                Projectile.position = owner.Center - Projectile.Size;
            }

            Vector2 idleTarget = owner.Center + new Vector2((Projectile.minionPos * 40 + 30) * owner.direction * -((Projectile.minionPos % 2) * 2 - 1), Projectile.minionPos * -10);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);

            if (foundTarget)
            {
                chargeCounter++;
                if (distanceFromTarget <= 300f)
                {
                    if (chargeCounter > 60)
                    {
                        chargeCounter = 0;
                        Projectile.velocity = Projectile.DirectionTo(targetCenter) * 20f;
                    }
                    Projectile.velocity *= 0.98f;
                }
                else
                {
                    Projectile.velocity += Projectile.DirectionTo(targetCenter);
                    Projectile.velocity *= 0.93f;
                }
            }
            else
            {
                if (Projectile.Distance(idleTarget) > 30f)
                {
                    Projectile.velocity += Projectile.DirectionTo(idleTarget);
                    Projectile.velocity *= 0.93f;
                }
            }

            Projectile.rotation += Math.Sign(Projectile.velocity.X) * (Projectile.velocity.Length() + 1f) / 20f;
        }

        //Taken from ExampleMod because I couldn't be bothered to write it myself lmao (thank you ExampleMod people!)
        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            Projectile.friendly = foundTarget;
        }
    }
}
