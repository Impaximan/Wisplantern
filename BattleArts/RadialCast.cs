using Wisplantern.ID;
using System;
using Terraria.DataStructures;

namespace Wisplantern.BattleArts
{
    class RadialCast : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.RadialCast>();

        public override int ID => BattleArtID.RadialCast;

        const int amount = 12;

        public override string BattleArtDescription => "Right click to fire " + amount + " spells at once, radially" +
            "\nCooldown scales with use speed";

        public override string BattleArtName => "Radial Cast";

        public override BattleArtType BattleArtType => BattleArtType.Magic;

        public override Color Color => Color.Pink;

        bool alreadyDoneExtraShot = false;
        public override void PreUseBattleArt(ref Item item, Player player)
        {
            alreadyDoneExtraShot = false;
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), item.useTime * 25);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!alreadyDoneExtraShot) //To prevent the game from freezing
            {
                alreadyDoneExtraShot = true;
                
                for (int i = 1; i < amount; i++)
                {
                    ShootExtraShot(item, velocity.RotatedBy(Math.PI / amount * i * 2f), position, player);
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
