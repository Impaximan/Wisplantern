using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Wisplantern.Globals.GNPCs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;

namespace Wisplantern
{
    abstract class DecoyItem : ModItem
    {
        /// <summary>
        /// The type of NPC this will spawn.
        /// </summary>
        public virtual int DecoyType => NPCID.None;

        /// <summary>
        /// The standard SetDefaults, DON'T USE THIS FOR DECOY ITEMS. Use SetStats instead.
        /// </summary>
        public override void SetDefaults()
        {
            Item.shoot = 1;
            SetStats();
        }

        /// <summary>
        /// Use this instead of SetDefaults. Don't set Item.shoot;
        /// </summary>
        public virtual void SetStats()
        {

        }

        /// <summary>
        /// Don't override this for decoy items unless you want to have custom spawning behavior.
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
            int n = NPC.NewNPC(source, (int)player.Center.X, (int)player.Center.Y, DecoyType);
            Main.npc[n].position.Y = player.position.Y + player.height - Main.npc[n].height;
            Main.npc[n].ai[0] = player.whoAmI;
            Main.npc[n].ai[1] = player.GetWeaponCrit(Item);
            Main.npc[n].knockBackResist = 0f;
            Main.npc[n].damage = damage;
            return false;
        }
    }
    abstract class DecoyNPC : ModNPC
    {
        /// <summary>
        /// The standard SetDefaults, DON'T USE THIS FOR DECOYS. Use SetStats instead.
        /// </summary>
        public override void SetDefaults()
        {
            NPC.GetGlobalNPC<InfightingNPC>().decoy = true;
            NPC.friendly = true;
            NPC.aiStyle = -1;
            SetStats();
        }

        public virtual void SetStats()
        {

        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return null;
        }

        public virtual bool DoContactDamage => true;

        /// <summary>
        /// Don't override this for DecoyNPCs.
        /// </summary>
        public override void PostAI()
        {
            if (DoContactDamage)
            {
                foreach (NPC target in Main.npc)
                {
                    if (target.active && target.whoAmI != NPC.whoAmI && target.Hitbox.Intersects(NPC.Hitbox) && target.GetGlobalNPC<InfightingNPC>().infightIframes <= 0 && !target.friendly)
                    {
                        int damage = Main.DamageVar(NPC.damage, Main.player[(int)NPC.ai[0]].luck);
                        int struckDamage = (int)target.StrikeNPC(damage, 0f, Math.Sign(target.Center.X - NPC.Center.X), Main.rand.NextBool((int)NPC.ai[1], 100));
                        Main.player[(int)NPC.ai[0]].addDPS(struckDamage);
                        target.GetGlobalNPC<InfightingNPC>().infightIframes = 10;
                    }
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool SpecialOnKill()
        {
            return true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.RemoveWhere(x => true, true);
        }
    }
}
