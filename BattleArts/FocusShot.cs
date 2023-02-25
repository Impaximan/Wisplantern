using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Wisplantern.Globals.GItems;
using Terraria.DataStructures;

namespace Wisplantern.BattleArts
{
    class FocusShot : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.FocusShot>();

        public override int ID => BattleArtID.FocusShot;

        public override string BattleArtDescription => "Right click to focus all bullets directly towards the cursor and with increased velocity and damage" +
            "\n5 second cooldown";

        public override string BattleArtName => "Focus Shot";

        public override BattleArtType BattleArtType => BattleArtType.Gun;

        public override Color Color => Color.Pink;

        public override void PreUseBattleArt(ref Item item, Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), 60 * 5);
            SoundEngine.PlaySound(new SoundStyle("Wisplantern/Sounds/Effects/Gunfire5"), player.Center);
        }
    }

    class FocusShotProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo)
            {
                if (projectile.owner == Main.myPlayer)
                {
                    EntitySource_ItemUse_WithAmmo nSource = (EntitySource_ItemUse_WithAmmo)source;
                    if (nSource.Item.GetGlobalItem<BattleArtItem>().battleArt is FocusShot && nSource.Item.GetGlobalItem<BattleArtItem>().ShouldApplyBattleArt(nSource.Entity as Player))
                    {
                        projectile.velocity = projectile.DirectionTo(Main.MouseWorld) * projectile.velocity.Length() * 2f;
                        projectile.knockBack *= 1.5f;
                        projectile.damage = (int)(projectile.damage * 1.5f);
                    }
                }
            }
        }
    }
}
