using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.ID;
using Terraria.Utilities;

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
                TooltipLine scholarLine = new TooltipLine(Mod, "ScholarsLensTooltip", scholarsDescription);
                scholarLine.OverrideColor = Color.Goldenrod;
                tooltips.Add(scholarLine);
            }
        }
    }
}
