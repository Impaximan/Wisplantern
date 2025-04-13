global using Terraria;
global using Terraria.ModLoader;
global using Terraria.ID;
global using Microsoft.Xna.Framework;
global using Wisplantern.ModPlayers;
global using System.Collections.Generic;
global using System;
global using Wisplantern.EasyPackets;
global using EasyPacketsLib;
using ReLogic.Content;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace Wisplantern
{
    public class Wisplantern : Mod
    {
        public static Wisplantern instance;

        #region Image Generation
        public virtual void SetColors(ref Color[] colors, int width, int height)
        {
            FastNoiseLite noise = new();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            int centerX = width / 2;
            int centerY = height / 2;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Vector2 center = new(centerX, centerY);
                    Vector2 position = new(i, j);
                    float rotation = (position - center).ToRotation();
                    float distance = Vector2.Distance(position, center);

                    int index = j * width + i;
                    Vector2 noisePosition = rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(distance / 2f)) * 1000f;
                    float noiseValue = (noise.GetNoise(noisePosition.X, noisePosition.Y) + 1f) / 2f;
                    noiseValue = 1f;
                    float glowValue = 1f - (distance / (width / 2f));
                    float value = MathHelper.Lerp(glowValue, noiseValue, distance / (width / 2f)) * glowValue;

                    colors[index] = new Color(value, value, value, value);
                }
            }
        }

        public virtual Texture2D CreateImage(int width, int height)
        {
            var graphics = Main.instance.GraphicsDevice;
            Color[] colors = new Color[width * height];
            Texture2D output = new(graphics, width, height, false, SurfaceFormat.Color);
            SetColors(ref colors, width, height);
            output.SetData(colors);
            return output;
        }

        #endregion

        public static int freezeFrames = 0;
        public static bool freezeFrameLight = false;

        public static List<int> wisplanternLoot = new();

        private static string savingFolder = Path.Combine(Main.SavePath, "Mods", "Cache");

        //Config stuff
        public static bool generateMassiveMountain = true;
        public static bool generatePits = true;
        public static bool generateLushCaves = true;
        public static bool generateVolcanicCaves = true;

        public static bool playDeepUnderground = true;
        public static bool playSnowstorm = true;
        public static bool playClairDeLune = true;

        public static bool classTags = false;

        public static bool calmNights = true;
        public static bool snowstorms = true;

        public static float charismaX = 0f;
        public static float charismaY = 0f;
        public static CharismaBehavior charismaBehavior = CharismaBehavior.ShowOnlyWithManipulative;
        public static bool pushCharismaDown = true;

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

            instance = this;

            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/StoneHit1", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/StoneHit2", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/HeavyHit", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/HeavyMetal", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/SwordUnsheath", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/Enchant1", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/Enchant2", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/Enchant3", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/Gunfire5", AssetRequestMode.ImmediateLoad);

            wisplanternLoot.Add(ModContent.ItemType<Items.Equipable.Accessories.WispNecklace>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Equipable.Accessories.WispRing>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Tools.FishingPoles.Wispcaster>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Tools.Pickaxes.HyperstonePick>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Tools.Movement.Hooklantern>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Weapons.Ranged.Bows.OtherBow>());
            wisplanternLoot.Add(ModContent.ItemType<Items.Weapons.Manipulative.Canes.GuidingLight>());

            if (Terraria.Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> winterRef = new((Effect)ModContent.Request<Effect>("Wisplantern/Effects/WinterShader", AssetRequestMode.ImmediateLoad));
                Filters.Scene["Wisplantern:WinterShader"] = new Filter(new ScreenShaderData(winterRef, "Winter"), EffectPriority.VeryHigh);
                Filters.Scene["Wisplantern:WinterShader"].Load();
            }

            Detours.Load();
        }

        public override void Unload()
        {
            Detours.Unload();
            wisplanternLoot.Clear();
        }
    }
}