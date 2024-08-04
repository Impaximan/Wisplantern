using Terraria.DataStructures;
using System.Collections.Generic;


namespace Wisplantern.Items.Weapons.Summon.Minions
{
    abstract class MinionStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;
        }

        public override void SetDefaults()
        {

        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            MinionPlayer modPlayer= player.GetModPlayer<MinionPlayer>();
            modPlayer.minionHandlers.Add(new MinionHandler(Item.shoot, Item.buffType));
            player.AddBuff(Item.buffType, 2);

            Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            return false;
        }
    }

    class MinionPlayer : ModPlayer
    {
        public List<MinionHandler> minionHandlers = new();

        public override void PostUpdateBuffs()
        {
            if (minionHandlers.Count > 0)
            {
                for (int i = 0; i < minionHandlers.Count; i++)
                {
                    if (i >= minionHandlers.Count)
                    {
                        break;
                    }

                    MinionHandler handler = minionHandlers[i];

                    if (handler.UpdateRemove(Player))
                    {
                        minionHandlers.RemoveAt(i);
                        i--;
                    }
                }

                //Maybe unneccessary?
                //float minionsFound = 0f;
                //for (int i = 0; i < Main.maxProjectiles; i++)
                //{
                //    Projectile projectile = Main.projectile[i];
                //    if (projectile.active && projectile.minionSlots > 0f && projectile.owner == Player.whoAmI)
                //    {
                //        minionsFound += projectile.minionSlots;
                //        if (minionsFound > Player.maxMinions)
                //        {
                //            projectile.Kill();
                //            projectile.active = false;
                //        }
                //    }
                //}
            }
        }
    }

    class MinionHandler
    {
        public int minionProjectileID;
        public int buffID;

        public MinionHandler(int minionProjectileID, int buffID)
        {
            this.minionProjectileID = minionProjectileID;
            this.buffID = buffID;
        }

        public bool UpdateRemove(Player player)
        {
            if (player.HasBuff(buffID))
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == minionProjectileID)
                    {
                        player.AddBuff(buffID, 2);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == minionProjectileID)
                    {
                        Main.projectile[i].active = false;
                    }
                }

                return true;
            }
            return false;
        }
    }
}
