using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace Wisplantern.BattleArts
{
    class BloodySlash : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.BloodySlash>();

        public override int ID => BattleArtID.BloodySlash;

        public override bool? CanHitNPC(Item item, Player player, NPC target)
        {
            return false;
        }

        public override string BattleArtDescription => "Right click to consume 5% of your health and slash with blood at enemies" +
            "\nAfflicts foes with a powerful stacking 'bloodbath' debuff";

        public override string BattleArtName => "Bloody Slash";

        public override BattleArtType BattleArtType => BattleArtType.Sword;

        public override Color Color => Color.LightPink;

        public override void PreUseBattleArt(ref Item item, Player player)
        {
            item.noUseGraphic = true;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.Shoot;
            item.shoot = ModContent.ProjectileType<BloodySlashProjectile>();
            item.shootSpeed += 2f;
            item.useTime = item.useAnimation;
            player.statLife -= player.statLifeMax2 / 20;
            CombatText.NewText(player.getRect(), Color.Red, player.statLifeMax2 / 20, false, true);
            SoundEngine.PlaySound(player.Male ? SoundID.PlayerHit : SoundID.FemaleHit, player.Center);
            if (player.statLife <= 0)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " bled themself to death."), player.statLifeMax2 / 20, 0);
            }
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage = (int)(damage / ((float)item.useTime / 20f));
        }

        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {

        }

        public override void UseBattleArt(Item item, Player player, bool firstFrame)
        {

        }

        public override void PostUseBattleArt(Item item, Player player)
        {
            item.noUseGraphic = false;
        }

        public override void PostUpdatePlayer(Player player)
        {

        }
    }

    class BloodySlashProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 58;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = Main.rand.Next(2);
            Projectile.ai[1] = Main.rand.NextFloat(0.5f, 0.75f);
            Projectile.scale = Main.rand.NextFloat(1.2f, 1.5f);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Vector2 center = hitbox.Center();
            hitbox.Width = (int)(50 * Projectile.ai[1]);
            hitbox.Height = (int)(50 * Projectile.ai[1]);
            hitbox.X = (int)center.X - hitbox.Width / 2;
            hitbox.Y = (int)center.Y - hitbox.Height / 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.ai[0] == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 52, 58), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(52 / 2, 58 / 2), new Vector2(1f, Projectile.ai[1]) * Projectile.scale, effects, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            BloodySlashNPC sNPC = target.GetGlobalNPC<BloodySlashNPC>();
            sNPC.stacks.Add((int)(damage * 1.8f));
            sNPC.timeToDecreaseStack = 120;
        }

        public override void AI()
        {
            Projectile.alpha += 255 / 60;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }

    class BloodySlashNPC : GlobalNPC
    {
        public int timeToDecreaseStack = 0;
        public List<int> stacks = new List<int>();

        public override bool InstancePerEntity => true;

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (stacks.Count > 0)
            {
                foreach (int stack in stacks)
                {
                    npc.lifeRegen -= stack;
                    damage += stack / 15 + 1;
                }
            }
        }

        public override void PostAI(NPC npc)
        {
            if (timeToDecreaseStack > 0)
            {
                timeToDecreaseStack--;
                if (timeToDecreaseStack <= 0 && stacks.Count > 0)
                {
                    timeToDecreaseStack = 30;
                    stacks.RemoveAt(0);
                }
            }
        }
    }
}