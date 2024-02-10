using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Wisplantern.Globals.GNPCs;
using ReLogic.Content;

namespace Wisplantern.UI.AggravationMeter
{
    internal class AggravationMeterUI : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D aggravationMeter = ModContent.Request<Texture2D>("Wisplantern/UI/AggravationMeter/AggravationLevel", AssetRequestMode.ImmediateLoad).Value;
            foreach (NPC npc in Main.npc)
            {
                if (npc != null && npc.active && npc.TryGetGlobalNPC(out InfightingNPC iNPC))
                {
                    if (iNPC.aggravation > 0f)
                    {
                        Vector2 positionOnScreen = npc.Center - Main.screenPosition;
                        positionOnScreen.Y -= npc.height + 5;

                        int frameHeight = (int)(18f * iNPC.aggravation);
                        int frameY = (iNPC.aggravated ? 18 : 0);
                        spriteBatch.Draw(aggravationMeter, positionOnScreen, new Rectangle(0, frameY + 18 - frameHeight, 18, frameHeight), Color.White * 0.75f, 0f, new Vector2(9, frameHeight), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
