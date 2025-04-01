using Wisplantern.Items.Equipable.Accessories;

namespace Wisplantern.Globals.GNPCs
{
    public class AccessoryNPC : GlobalNPC
    {
        public override bool InstancePerEntity => false;

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (player.AccessoryActive<Gasoline>() && item.DamageType is DamageClasses.ManipulativeDamageClass)
            {
                npc.AddBuff(BuffID.OnFire, 60 * 6);
            }
        }
    }
}
