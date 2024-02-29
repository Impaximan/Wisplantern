using Terraria.DataStructures;
using Terraria.Audio;

namespace Wisplantern.Items.Weapons.Manipulative.Incantations
{
    public abstract class Incantation : ModItem
    {
        /// <summary>
        /// Don't override this for incantations, use IncantationSetDefaults instead.
        /// Automatically sets useStyle, shoot, noMelee, knockBack, UseSound, autoReuse, and shootSpeed.
        /// </summary>
        public sealed override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.shoot = 1;
            Item.knockBack = 3f;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.DamageType = ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>();
            IncantationSetDefaults();
        }

        /// <summary>
        /// You know how it is.
        /// </summary>
        public virtual void IncantationSetDefaults()
        {

        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }


        /// <summary>
        /// Don't override this for incantations.
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
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.GetGlobalNPC<Globals.GNPCs.InfightingNPC>().aggravated)
                {
                    IncantationEffect(player, source, position, npc, damage);
                }
            }
            return false;
        }

        public virtual void IncantationEffect(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 targetPosition, NPC npc, int damage)
        {

        }
    }
}
