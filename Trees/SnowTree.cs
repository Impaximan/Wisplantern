using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;

namespace Wisplantern.Trees
{
    class SnowTree : ModTree
    {
		public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {

        }

        public override void SetStaticDefaults()
		{
			GrowsOnTileId = new int[1] { ModContent.TileType<Tiles.SnowGrass>() };
		}

		public override Asset<Texture2D> GetTexture()
		{
			return ModContent.Request<Texture2D>("Wisplantern/Trees/SnowTree");
		}

		public override Asset<Texture2D> GetBranchTextures()
		{
			return ModContent.Request<Texture2D>("Wisplantern/Trees/SnowTree_Branches");
		}

		public override Asset<Texture2D> GetTopTextures()
		{
			return ModContent.Request<Texture2D>("Wisplantern/Trees/SnowTree_Tops");
		}

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return TileID.Saplings;
		}

		public override int DropWood()
		{
			return ItemID.Wood;
		}
	}
}
