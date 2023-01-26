using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Wisplantern.ID;
using Microsoft.Xna.Framework;

namespace Wisplantern.BattleArts
{
    class Parry : BattleArt
    {
        public override int ID => BattleArtID.Parry;

        public override string BattleArtDescription => "Right click right before an attack hits you to negate 80% of the damage and knock nearby enemies away";

        public override string BattleArtName => "Sword Parry";

        public override BattleArtType BattleArtType => BattleArtType.Sword;

        public override Color Color => Color.SkyBlue;

        public override void PreUseBattleArt(ref Item item, Player player)
        {
            item.useTime = 10;
            item.useAnimation = 10;
            item.shoot = ProjectileID.None;
            //TODO: Make it actually parry
        }

        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            player.itemRotation = MathHelper.ToRadians(65) * player.direction;
            player.itemLocation = player.Center + new Vector2(0, -14);
        }

        public override void PostUpdatePlayer(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 2;
        }
    }
}