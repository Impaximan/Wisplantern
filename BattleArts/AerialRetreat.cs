using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using Terraria.Graphics.CameraModifiers;
using Terraria.Audio;

namespace Wisplantern.BattleArts
{
    class AerialRetreat : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.AerialRetreat>();

        public override int ID => BattleArtID.AerialRetreat;

        public override string BattleArtDescription => "Right click to flee into the air, briefly giving yourself featherfall" +
            "\n15 second cooldown";

        public override string BattleArtName => "Aerial Retreat";

        public override BattleArtType BattleArtType => BattleArtType.BowAndRepeater;

        public override Color Color => Color.SkyBlue;

        float launchAmount = 10f;

        float rotationAddend = 0f;
        public override void PreUseBattleArt(ref Item item, Player player)
        {
            item.damage *= 2;
            player.velocity.Y -= launchAmount;
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), 60 * 15);
            player.AddBuff(BuffID.Featherfall, 60 * 7);
            player.immuneTime = 90;
            player.immune = true;
            player.jump = 0;
            SoundEngine.PlaySound(SoundID.Item102, player.Center);
        }
    }
}
