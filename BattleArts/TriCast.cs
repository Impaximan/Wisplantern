using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using Terraria.Graphics.CameraModifiers;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Wisplantern.BattleArts
{
    class TriCast : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.TriCast>();

        public override int ID => BattleArtID.TriCast;

        public override string BattleArtDescription => "Right click to fire 3 spells at once" +
            "\nCooldown scales with item use speed";

        public override string BattleArtName => "Tri-Cast";

        public override BattleArtType BattleArtType => BattleArtType.Magic;

        public override Color Color => Color.Pink;

        bool alreadyDoneExtraShot = false;
        public override void PreUseBattleArt(ref Item item, Player player)
        {
            alreadyDoneExtraShot = false;
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), item.useTime * 10);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!alreadyDoneExtraShot) //To prevent the game from freezing
            {
                alreadyDoneExtraShot = true;
                ShootExtraShot(item, velocity.RotatedBy(-MathHelper.Pi / 8f), position, player);
                ShootExtraShot(item, velocity.RotatedBy(MathHelper.Pi / 8f), position, player);
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
