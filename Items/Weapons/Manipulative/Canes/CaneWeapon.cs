using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;

namespace Wisplantern.Items.Weapons.Manipulative.Canes
{
    abstract class CaneWeapon : ModItem
    {

        /// <summary>
        /// Don't override this for canes, use CaneSetDefaults instead.
        /// Automatically sets useStyle, shoot, noMelee, knockBack, UseSound, autoReuse, and shootSpeed.
        /// </summary>
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = 1;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item152;
            Item.autoReuse = true;
            //Item.shootSpeed = 1f;
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                return false;
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(player.Center) <= MaxDistance)
                {
                     if (Item.AggravateNPC(npc, player)) OnAggravate(npc);
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
            SoundEngine.PlaySound(SoundID.Item8, player.Center);
            return false;
        }

        public virtual void OnAggravate(NPC npc)
        {

        }
    }
}
