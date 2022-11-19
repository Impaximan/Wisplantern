using Terraria.ModLoader;
using ReLogic.Content;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Wisplantern
{
	public class Wisplantern : Mod
    {
        #region Image Generation
        public virtual void SetColors(ref Color[] colors, int width, int height)
        {
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            int centerX = width / 2;
            int centerY = height / 2;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Vector2 center = new Vector2(centerX, centerY);
                    Vector2 position = new Vector2(i, j);
                    float rotation = (position - center).ToRotation();
                    float distance = Vector2.Distance(position, center);

                    int index = j * width + i;
                    Vector2 noisePosition = rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(distance / 2f)) * 1000f;
                    float noiseValue = (noise.GetNoise(noisePosition.X, noisePosition.Y) + 1f) / 2f;
                    noiseValue = 1f;
                    float glowValue = (1f - (distance / (width / 2f)));
                    float value = MathHelper.Lerp(glowValue, noiseValue, distance / (width / 2f)) * glowValue;

                    colors[index] = new Color(value, value, value, value);
                }
            }
        }

        public virtual Texture2D CreateImage(int width, int height)
        {
            var graphics = Main.instance.GraphicsDevice;
            Color[] colors = new Color[width * height];
            Texture2D output = new Texture2D(graphics, width, height, false, SurfaceFormat.Color);
            SetColors(ref colors, width, height);
            output.SetData(colors);
            return output;
        }

        #endregion

        public static int freezeFrames = 0;
        public static bool freezeFrameLight = false;

        public static List<int> wisplanternLoot = new List<int>();

        private static string savingFolder = Path.Combine(Main.SavePath, "Mods", "Cache");
        public override void Load()
        {
            //WispUtils.InvokeOnMainThread(() =>
            //{
            //    Directory.CreateDirectory(savingFolder);
            //    string path = Path.Combine(savingFolder, "Bloom.png");
            //    using (Stream stream = File.OpenWrite(path))
            //    {
            //        CreateImage(1000, 1000).SaveAsPng(stream, 1000, 1000);
            //    }
            //});

            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/StoneHit1", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/StoneHit2", AssetRequestMode.ImmediateLoad);

            wisplanternLoot.Add(ModContent.ItemType<Items.Equipable.Accessories.WispNecklace>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Equipable.Accessories.WispRing>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Tools.FishingPoles.Wispcaster>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Tools.Pickaxes.HyperstonePick>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Weapons.Ranged.Bows.OtherBow>());

            Detours.Load();
        }

        public override void Unload()
        {
            Detours.Unload();
            wisplanternLoot.Clear();
        }
    }
}