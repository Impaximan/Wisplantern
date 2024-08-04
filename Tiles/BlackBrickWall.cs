using Terraria.Audio;

namespace Wisplantern.Tiles
{
	public class BlackBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Stone;

			SoundStyle style = new("Wisplantern/Sounds/Effects/StoneHit2");
			style.PitchVariance = 0.25f;
			HitSound = style;

			AddMapEntry(new Color(83 / 2, 72 / 2, 69 / 2));
		}

		//public override void NumDust(int i, int j, bool fail, ref int num)
		//{
		//	num = fail ? 1 : 3;
		//}
	}
}