using Terraria.DataStructures;
using Terraria.Audio;
using Wisplantern.Items.Weapons.Manipulative.Decoys;
using Wisplantern.DamageClasses;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    abstract class CaneWeapon : ModItem
    {
        public override bool WeaponPrefix()
        {
            return true;
        }

        /// <summary>
        /// Don't override this for canes, use CaneSetDefaults instead.
        /// Automatically sets useStyle, shoot, noMelee, knockBack, UseSound, autoReuse, and shootSpeed.
        /// </summary>
        public sealed override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<ManipulativeDamageClass>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = 1;
            Item.shootSpeed = 1f;
            Item.knockBack = 3f;
            SoundStyle style = SoundID.Item152;
            style.PitchVariance = 0.5f;
            style.MaxInstances = 0;
            Item.UseSound = style;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.ammo = 0;
            Item.useTime = 10;
            Item.useAnimation = 10;
            CaneSetDefaults();
        }

        /// <summary>
        /// You know how it is.
        /// </summary>
        public virtual void CaneSetDefaults()
        {

        }

        public override bool? CanHitNPC(Player player, NPC target)
        {
            if (player.altFunctionUse == 2 && Item.GetGlobalItem<Globals.GItems.BattleArtItem>().battleArt is BattleArts.FinishOff)
            {
                return base.CanHitNPC(player, target);
            }
            return false;
        }

        /// <summary>
        /// The radius of the cane's effect.
        /// </summary>
        public virtual float MaxDistance => 225f;

        /// <summary>
        /// The type of dust used to show the cane's effect.
        /// </summary>
        public virtual int DustType => DustID.PurpleTorch;

        /// <summary>
        /// Don't override this for canes.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <returns></returns>
        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                return false;
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(player.Center) <= MaxDistance)
                {
                     if (Item.AggravateNPC(npc, player)) OnAggravate(npc, player);
                }
            }


            for (int i = 0; i < 150; i++)
            {
                Vector2 dustPos = Main.rand.NextVector2CircularEdge(MaxDistance, MaxDistance) + player.Center;
                if (!Collision.SolidCollision(dustPos + player.velocity * 8f, 1, 1) && !Collision.SolidCollision(dustPos, 1, 1))
                {
                    int dust = Dust.NewDust(dustPos, 0, 0, DustType);
                    Main.dust[dust].velocity = player.velocity;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLightEmittence = true;
                    Main.dust[dust].noLight = true;
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer) Mod.SendPacket(new CaneCircle(player.Center.X, player.Center.Y, MaxDistance, DustType, player.velocity.X, player.velocity.Y), -1, player.whoAmI, true);

            SoundStyle sound = SoundID.Item8;
            sound.PitchVariance = 0.15f;
            sound.MaxInstances = 4;
            sound.SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest;
            SoundEngine.PlaySound(sound, player.Center);
            return false;
        }

        public virtual void OnAggravate(NPC npc, Player player)
        {

        }
    }
}
