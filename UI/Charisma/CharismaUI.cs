using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Wisplantern.Globals.GNPCs;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using ReLogic.Graphics;

namespace Wisplantern.UI.Charisma
{
    internal class CharismaUI : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D charisma = ModContent.Request<Texture2D>("Wisplantern/UI/Charisma/Charisma", AssetRequestMode.ImmediateLoad).Value;

            Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) * new Vector2(Wisplantern.charismaX, Wisplantern.charismaY);

            if (Main.playerInventory && Wisplantern.pushCharismaDown)
            {
                position.Y += Main.UIScale * 50f;
            }

            Vector2 ogPosition = position;


            Player player = Main.LocalPlayer;
            ManipulativePlayer mPlayer = player.GetModPlayer<ManipulativePlayer>();

            //spriteBatch.Draw(charisma, /*position + new Vector2(0f, 2f * Main.UIScale)*/Vector2.Zero, new Rectangle(0, 0, charisma.Width, charisma.Height), Color.White, 0f, charisma.Size() / 2f, 10f * Main.UIScale, SpriteEffects.None, 0f);
            for (int i = 0; i < mPlayer.MaxCharisma; i++)
            {
                if (mPlayer.charisma > i)
                {
                    spriteBatch.Draw(charisma, position + new Vector2(0f, 2f * Main.UIScale), new Rectangle(0, 0, charisma.Width, charisma.Height), Color.White, 0f, charisma.Size() / 2f, 0.8f * Main.UIScale, SpriteEffects.None, 0f);
                }

                if (i != mPlayer.MaxCharisma - 1)
                {
                    position.X -= (charisma.Width + 5f) * Main.UIScale;
                }
            }

            string text = "Charisma: " + mPlayer.charisma + "/" + mPlayer.MaxCharisma;
            
            spriteBatch.DrawString(FontAssets.DeathText.Value, text, (ogPosition + position) / 2f + new Vector2(0f, -14f * Main.UIScale), new Color(0.9f, 0.9f, 0.9f, 1f), 0f, FontAssets.DeathText.Value.MeasureString(text) / 2f, Main.UIScale * 0.4f, SpriteEffects.None, 0f);
        }
    }
}
