namespace Wisplantern.Tiles
{
	public class SmoothDepthrockWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Stone;

			AddMapEntry(new Color(140 / 2, 118 / 2, 101 / 2));
		}

		//public override void NumDust(int i, int j, bool fail, ref int num)
		//{
		//	num = fail ? 1 : 3;
		//}
	}
}