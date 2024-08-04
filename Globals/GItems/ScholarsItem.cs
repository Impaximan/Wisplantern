using System.Collections.Generic;
using System;

namespace Wisplantern.Globals.GItems
{
    class ScholarsItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public string scholarsDescription = "None";

        public override void SetDefaults(Item item)
        {
            if (item.ModItem == null || item.ModItem.Mod.Name == "Terraria")
            {
                item.SetScholarlyDescription(item.type switch
                {
                    ItemID.RodofDiscord => "A chaotic rod, dropped extremely rarely from chaos elementals within the underground hallowed",
                    ItemID.TorchGodsFavor => "A gift from the very god of torches, to be cherished forever",
                    ItemID.ZapinatorGray => "Sold extremely rarely by the Traveling Merchant before the spirits of light and dark have been released",
                    ItemID.ZapinatorOrange => "Sold extremely rarely by the Traveling Merchant after the spirits of light and dark have been released",
                    ItemID.ArcaneRuneWall => "Sold extremely rarely by the Traveling Merchant",
                    ItemID.Kimono => "Sold extremely rarely by the Traveling Merchant",
                    ItemID.BlackCounterweight => "Sold extremely rarely by the Traveling Merchant",
                    ItemID.BouncingShield => "Sold extremely rarely by the Traveling Merchant after the spirits of light and dark have been released",
                    ItemID.Gatligator => "Sold extremely rarely by the Traveling Merchant after the spirits of light and dark have been released",
                    ItemID.HermesBoots => "Found in chests underground or in crates",
                    ItemID.CloudinaBottle => "Found in chests underground or in crates",
                    ItemID.MagicMirror => "Found in chests underground or in crates",
                    ItemID.BandofRegeneration => "Found in chests underground or in crates",
                    ItemID.Mace => "Found in chests underground or in crates",
                    ItemID.ShoeSpikes => "Found in chests underground or in crates",
                    ItemID.FlareGun => "Found in chests underground or in crates",
                    ItemID.Flare => "Found in chests underground or in crates",
                    ItemID.Extractinator => "Found in chests underground or in crates",
                    ItemID.AegisCrystal => "Obtained by throwing a life crystal into a special, astral liquid",
                    ItemID.AegisFruit => "Obtained by throwing a life fruit into a special, astral liquid",
                    ItemID.ArcaneCrystal => "Obtained by throwing a mana crystal into a special, astral liquid",
                    ItemID.Ambrosia => "Obtained by throwing fruit into a special, astral liquid",
                    ItemID.GummyWorm => "Obtained by throwing a worm into a special, astral liquid",
                    ItemID.GalaxyPearl => "Obtained by throwing a pink pearl into a special, astral liquid",
                    ItemID.CombatBookVolumeTwo => "Obtained by throwing a spell tome into a special, astral liquid",
                    ItemID.PeddlersSatchel => "Obtained by throwing a peddler's hat into a special, astral liquid",
                    ItemID.RodOfHarmony => "Obtained by throwing the Rod of Discord into a special, astral liquid, but only once an immense deity has been slain",
                    ItemID.Clentaminator2 => "Obtained by throwing the Clentaminator into a special, astral liquid, but only once an immense deity has been slain",
                    ItemID.ShimmerArrow => "Obtained by throwing wooden or hellfire arrows into a special, astral liquid",
                    ItemID.GasTrap => "Obtained by throwing a whoopie cushion into a special, astral liquid",
                    ItemID.ShimmerFlare => "Obtained by throwing flares into a special, astral liquid",
                    ItemID.ShimmerTorch => "Obtained by throwing torches into a special, astral liquid",
                    ItemID.ShimmerCampfire => "Obtained by throwing a campfire into a special, astral liquid",
                    ItemID.ShimmerMonolith => "Obtained by throwing an angel statue into a special, astral liquid",
                    ItemID.ShimmerCloak => "Obtained by throwing a star cloak into a special, astral liquid",
                    ItemID.WhoopieCushion => "Dropped, strangely, by worms underground. What are they planning...",
                    ItemID.CorruptionKey => "Dropped rarely by enemies in the Corruption",
                    ItemID.CrimsonKey => "Dropped rarely by enemies in the Crimson",
                    ItemID.HallowedKey => "Dropped rarely by enemies in the Hallow",
                    ItemID.FrozenKey => "Dropped rarely by enemies in the snow/ice biome",
                    ItemID.JungleKey => "Dropped rarely by enemies in the jungle biome",
                    ItemID.DungeonDesertKey => "Dropped rarely by enemies in the desert biome",
                    _ => "None",
                });
            }
        }

        public bool ShouldShowScholarsDesc()
        {
            return scholarsDescription != "None" && Main.LocalPlayer.GetModPlayer<Items.Info.ScholarlyPlayer>().equipped;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ShouldShowScholarsDesc())
            {
                TooltipLine scholarLine = new(Mod, "ScholarsLensTooltip", scholarsDescription);
                scholarLine.OverrideColor = Color.Goldenrod;
                tooltips.Add(scholarLine);
            }

            if (Main.LocalPlayer.GetModPlayer<Items.Info.ScholarlyPlayer>().equipped && item.damage >= 0)
            {
                if (tooltips.Find(x => x.Name == "Speed") != null)
                {
                    tooltips.Find(x => x.Name == "Speed").Text += " ([c/daa520:" + item.useTime + "])" +
                        "\nProjectile velocity: [c/daa520:" + item.shootSpeed + "]";
                }
                if (tooltips.Find(x => x.Name == "Knockback") != null)
                {
                    tooltips.Find(x => x.Name == "Knockback").Text += " ([c/daa520:" + Math.Round(item.knockBack, 1) + "])";
                }
            }
        }
    }
}
