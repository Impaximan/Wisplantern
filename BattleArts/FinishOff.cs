using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Wisplantern.BattleArts
{
    class FinishOff : BattleArt
    {
        public override int ItemType => ModContent.ItemType<Items.BattleArtItems.FinishOff>();

        public override int ID => BattleArtID.FinishOff;

        public override string BattleArtDescription => "Right click to hit enemies with the cane, instantly killing any enemy with less than 50% life left" +
            "\nInstantly kills bosses with less than 5% life left" +
            "\n15 second cooldown";

        public override string BattleArtName => "Finish Off";

        public override BattleArtType BattleArtType => BattleArtType.Cane;

        public override Color Color => Color.Pink;

        public override void PreUseBattleArt(ref Item item, Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.BattleArtCooldown>(), 60 * 15);
            item.noMelee = false;
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.boss && target.life <= target.lifeMax * 0.05f)
            {
                modifiers.SourceDamage.Base = target.life * 10;
                modifiers.SetCrit();
            }
            else if (!target.boss && target.life <= target.lifeMax * 0.5f)
            {
                modifiers.SourceDamage.Base = target.life * 10;
                modifiers.SetCrit();
            }
            else
            {
                modifiers.DisableCrit();
                modifiers.SourceDamage.Base = 1;
            }
        }
    }
}
