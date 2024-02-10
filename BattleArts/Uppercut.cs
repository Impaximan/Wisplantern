using Wisplantern.ID;

namespace Wisplantern.BattleArts
{
    class Uppercut : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.Uppercut>();

        public override int ID => BattleArtID.Uppercut;

        public override string BattleArtDescription => "An uppercut attack that launches both you and hit enemies into the air" +
            "\n3 second cooldown";

        public override string BattleArtName => "Uppercut";

        public override BattleArtType BattleArtType => BattleArtType.Sword;

        public override Color Color => Color.Pink;

        float launchAmount = 15f;

        float rotationAddend = 0f;
        public override void PreUseBattleArt(ref Item item, Player player)
        {
            item.shoot = ProjectileID.None;
            item.damage = (int)(item.damage * 1.5f);
            item.knockBack = 0;
            item.useTime /= 2;
            item.useAnimation /= 2;
            item.UseSound = SoundID.DD2_MonkStaffSwing;
            rotationAddend = 0f;
            player.velocity.Y -= launchAmount;
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), 60 * 3);
            player.jump = 0;
        }

        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            player.itemRotation = MathHelper.ToRadians(90 + rotationAddend) * player.direction;
            rotationAddend -= 180f / item.useTime;
            player.itemLocation = player.Center + new Vector2(0, -16);
        }

        public override bool? CanHitNPC(Item item, Player player, NPC target)
        {
            Rectangle hitbox = new ()
            {
                Width = item.width + 50,
                Height = item.height + 120
            };

            hitbox.X = (int)(player.Center.X + hitbox.Width * 0.5f * player.direction) - hitbox.Width / 2;
            hitbox.Y = (int)player.Bottom.Y - hitbox.Height + 20;

            return target.Hitbox.Intersects(hitbox);
        }

        public override void UseBattleArt(Item item, Player player, bool firstFrame)
        {

        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (target.knockBackResist != 0f)
            {
                target.velocity.Y -= launchAmount;
            }
            player.immuneTime = 60;
            player.immune = true;
            player.noFallDmg = true;
            NetMessage.SendData(MessageID.SyncNPC, number: target.whoAmI);
        }

        public override void PostUpdatePlayer(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 2;
        }
    }
}
