using Terraria.GameContent.Personalities;
using Terraria.GameContent;
using System.Linq;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;

namespace Wisplantern.NPCs.Town
{
	[AutoloadHead]
	public class Shepherd : ModNPC
	{
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 25;

			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 500;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4;
			//NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			//NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ExamplePersonEmote>();

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Velocity = 1f,
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Hate)
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Pirate, AffectionLevel.Like)
				.SetNPCAffection(633, AffectionLevel.Love) //Zoologist, couldn't find the ID
				;

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party")
			);
		}

        public override void SetDefaults()
        {
			NPC.width = 18;
			NPC.height = 40;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;

			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.damage = 8;
			NPC.defense = 3;
			NPC.lifeMax = 250;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;

			NPC.townNPC = true;
			NPC.friendly = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement("A shepherd of unknown origin, seemingly unrelated to everything around him. Appears to be a Satyr."),
			});
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
			for (int i = 0; i < Main.maxPlayers; i++)
            {
				Player player = Main.player[i];

				if (player != null && player.active)
                {
					if (player.inventory.Any(x => x.DamageType == ModContent.GetInstance<DamageClasses.ManipulativeDamageClass>() ||
					x.ModItem is Items.Weapons.Manipulative.Canes.CaneWeapon || 
					x.ModItem is Items.Weapons.Manipulative.Decoys.DecoyItem || 
					x.ModItem is Items.Weapons.Manipulative.Incantations.Incantation))
                    {
						return true;
                    }
                }
            }

            return false;
        }

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}

        public override List<string> SetNPCNameList()
        {
            return new()
            {
				"Advs. Saire",
				"Sixes",
				"Silenus",
				"Lamis",
				"Eumaeus",
				"Jack",
				"No hooves",
				"Immanuel",
				"David",
				"Amos"
            };
        }

        public override string GetChat()
        {
			List<string> potentialDialogues = new();

			int num = 1;
			while (Language.Exists("Mods.Wisplantern.Dialogue.Shepherd.Talk" + num.ToString()))
            {
				potentialDialogues.Add(Language.GetTextValue("Mods.Wisplantern.Dialogue.Shepherd.Talk" + num.ToString()));

				num++;
            }

            return Main.rand.Next(potentialDialogues);
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
		}

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
			if (firstButton)
            {
				shopName = "Shop";
            }
        }

		public override void AddShops()
		{
			new NPCShop(Type)
				.Add<Items.Weapons.Manipulative.Decoys.BouncyDummy>()
				.Add(new Item(ModContent.ItemType<Items.Materials.ScrollOfIncantation>()) { shopCustomPrice = Item.buyPrice(0, 4, 50, 0) })
				.Add<Items.Equipable.Accessories.Gasoline>()
				.Add<Items.Weapons.Manipulative.Canes.ShepherdsCane>()
				.Register();
		}

		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
	}
}
