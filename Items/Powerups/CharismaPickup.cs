using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wisplantern.Items.Powerups
{
    class CharismaPickup : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.IgnoresEncumberingStone[Type] = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1f - (Item.alpha / 255f));
        }

        float time = 0f;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            time++;
            Color color = new Color(252, 156, 80) * (1f + (float)Math.Sin(time / 20f) / 4f);

            Lighting.AddLight(Item.Center, color.R / 750f, color.G / 750f, color.B / 750f);
        }

        public override bool OnPickup(Player player)
        {
            player.GainCharisma(1);
            return false;
        }
    }
}
