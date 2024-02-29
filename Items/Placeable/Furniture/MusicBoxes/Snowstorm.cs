using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisplantern.Items.Placeable.Furniture.MusicBoxes
{
	public class Snowstorm : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
			ItemID.Sets.CanGetPrefixes[Type] = false;

			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SnowstormV2"), ModContent.ItemType<Snowstorm>(), ModContent.TileType<Tiles.MusicBoxes.Snowstorm>());
		}

		public override void SetDefaults()
		{
			Item.DefaultToMusicBox(ModContent.TileType<Tiles.MusicBoxes.Snowstorm>(), 0);
		}
	}
}
