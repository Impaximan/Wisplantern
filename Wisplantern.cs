using Terraria.ModLoader;
using ReLogic.Content;
using Microsoft.Xna.Framework.Audio;

namespace Wisplantern
{
	public class Wisplantern : Mod
	{
        public static int freezeFrames = 0;
        public static bool freezeFrameLight = false;

        public override void Load()
        {
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/StoneHit1", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("Wisplantern/Sounds/Effects/StoneHit2", AssetRequestMode.ImmediateLoad);

            Detours.Load();
        }

        public override void Unload()
        {
            Detours.Unload();
        }
    }
}