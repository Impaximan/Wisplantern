using Wisplantern.Globals.GNPCs;

namespace Wisplantern.Buffs
{
    class VisciousFrenzy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.5f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.2f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.TryGetGlobalNPC(out InfightingNPC iNPC))
            {
                iNPC.attackSpeedMult += 1f;
            }
        }
    }
}