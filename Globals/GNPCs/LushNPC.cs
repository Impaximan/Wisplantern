using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace Wisplantern.Globals.GNPCs
{
    class LushNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool? CanHitNPC(NPC npc, NPC target)
        {
            if (target.type == ModContent.NPCType<NPCs.Critters.Bugs.Lushfly>())
            {
                return false;
            }
            return base.CanHitNPC(npc, target);
        }
    }
}
