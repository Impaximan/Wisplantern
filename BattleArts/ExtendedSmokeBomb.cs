using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Wisplantern.BattleArts
{
    class ExtendedSmokeBomb : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.ExtendedSmokeBomb>();

        public override int ID => BattleArtID.ExtendedSmokeBomb;

        public override string BattleArtDescription => "Right click to smoke bomb yourself for 5 seconds" +
            "\n20 second cooldown";

        public override string BattleArtName => "Extended Smoke Bomb";

        public override BattleArtType BattleArtType => BattleArtType.Cane;

        public override Color Color => Color.SkyBlue;

        public override void PreUseBattleArt(ref Item item, Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), 60 * 20);
            player.SmokeBomb(300);
        }
    }
}
