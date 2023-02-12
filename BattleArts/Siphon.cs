using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Wisplantern.BattleArts
{
    class Siphon : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.Siphon>();

        public override int ID => BattleArtID.Siphon;

        public override string BattleArtDescription => "Right click to fire a faster, more powerful magic attack" +
            "\nIf it kills an enemy, that enemy is guarenteed to drop hearts and mana stars" +
            "\n6 second cooldown";

        public override string BattleArtName => "Siphon";

        public override BattleArtType BattleArtType => BattleArtType.Magic;

        public override Color Color => Color.LightGreen;

        public override void PreUseBattleArt(ref Item item, Player player)
        {
            player.GetModPlayer<SiphonPlayer>().siphoning = true;
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), 60 * 6);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity *= 2f;
            damage = (int)(damage * 1.5f);
        }
    }

    class SiphonPlayer : ModPlayer
    {
        public bool siphoning = false;
        public override void ResetEffects()
        {
            siphoning = false;
        }
    }

    class SiphonProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool siphoning = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            siphoning = false;
            if (source is EntitySource_ItemUse_WithAmmo)
            {
                EntitySource_ItemUse_WithAmmo newSource = source as EntitySource_ItemUse_WithAmmo;
                if (newSource.Entity is Player)
                {
                    Player player = newSource.Entity as Player;
                    if (player.GetModPlayer<SiphonPlayer>().siphoning)
                    {
                        siphoning = true;
                    }
                }
            }
        }

        public override void AI(Projectile projectile)
        {
            base.AI(projectile);
            if (siphoning) Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.LifeDrain, projectile.velocity.X / 2f, projectile.velocity.Y / 2f, Scale: 1.5f);
        }
    }

    class SiphonNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (projectile.GetGlobalProjectile<SiphonProjectile>().siphoning && npc.life <= 0)
            {
                for (int i = 0; i < Main.rand.Next(1, 3); i++)
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ItemID.Heart);
                }
                for (int i = 0; i < Main.rand.Next(1, 3); i++)
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ItemID.Star);
                }
            }
        }
    }
}
