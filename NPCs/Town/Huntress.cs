using Terraria.GameContent.Personalities;
using Terraria.GameContent;
using System.Linq;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Wisplantern.Systems;
using Wisplantern.Items.Weapons.Ranged.Misc;
using Wisplantern.Items.Ammo;
using Wisplantern.Items.Weapons.Ranged.Javelins;

namespace Wisplantern.NPCs.Town
{
    [AutoloadHead]
    public class Huntress : ModNPC
    {
        private static Profiles.StackedNPCProfile NPCProfile;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23;

            NPCID.Sets.ExtraFramesCount[Type] = 7;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 500;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4;
            //NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
            //NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ExamplePersonEmote>();

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 1f,
                Direction = -1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate)
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Hate)
                .SetNPCAffection(ModContent.NPCType<Shepherd>(), AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
                .SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love)
                ;

        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 17;
            NPC.defense = 3;
            NPC.lifeMax = 250;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Nurse;

            NPC.townNPC = true;
            NPC.friendly = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

                new FlavorTextBestiaryInfoElement("A huntress from the astral plane. Thought to be a descendant of some ancient god."),
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return Misc.HuntressSaved;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                string variant = "";
                int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
                int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
                int shoulderGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Shoulder").Type;
                int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;


                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, shoulderGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
            }
        }

        public override void PostAI()
        {
            Misc.HuntressSaved = true;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return null;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<PrimitiveDartProjectile>();
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
            gravityCorrection = 0.5f;
            randomOffset = 1f;
        }

        public override List<string> SetNPCNameList()
        {
            return new()
            {
                "Artemis",
                "Diana",
                "Athena",
                "Cassiopeia",
                "Andromeda",
                "Carina",
                "Columba",
                "Lyra",
                "Norma"
            };
        }

        public override string GetChat()
        {
            List<string> potentialDialogues = new();

            int num = 1;
            while (Language.Exists("Mods.Wisplantern.Dialogue.Huntress.Talk" + num.ToString()))
            {
                potentialDialogues.Add(Language.GetTextValue("Mods.Wisplantern.Dialogue.Huntress.Talk" + num.ToString()));

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
            NPCShop.Entry leather = new(ItemID.Leather);
            leather.Item.value = Item.buyPrice(0, 0, 50, 0);

            new NPCShop(Type)
                .Add<Atlatl>()
                .Add<HuntingJavelin>()
                .Add<PrimitiveDart>()
                .Add(leather)
                .Register();
        }

        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;
    }
}
