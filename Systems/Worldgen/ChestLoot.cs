using Wisplantern.Items.BattleArtItems;
using System.Collections.Generic;
using Terraria.WorldBuilding;

namespace Wisplantern.Systems.Worldgen
{
    class ChestLoot : ModSystem
    {
        public override void PostWorldGen()
        {
            for (int i = 0; i < 1000; i++)
            {
                Chest chest = Main.chest[i];

                List<int> availableBattleArts = new()
                {
                    ModContent.ItemType<SwordParry>(),
                    ModContent.ItemType<AerialRetreat>(),
                    ModContent.ItemType<Uppercut>(),
                    ModContent.ItemType<BloodySlash>(),
                    ModContent.ItemType<TriCast>(),
                    ModContent.ItemType<Siphon>(),
                    ModContent.ItemType<RadialCast>(),
                    ModContent.ItemType<ExtendedSmokeBomb>(),
                    ModContent.ItemType<FinishOff>(),
                    ModContent.ItemType<FocusShot>(),
                };

                if (chest != null)
                {
                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers) //All chests
                    {
                        if (WorldGen.genRand.NextBool(3))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Ranged.Javelins.HuntingJavelin>());
                                    chest.item[inventoryIndex].stack = Main.rand.Next(15, 41);
                                    break;
                                }
                            }
                        }

                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Manipulative.Decoys.BouncyDummy>());
                                    break;
                                }
                            }
                        }

                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(Main.rand.Next(availableBattleArts));
                                    break;
                                }
                            }
                        }

                        if (chest.y > GenVars.lavaLine && WorldGen.genRand.NextBool(6))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.DraconicDacron>());
                                    break;
                                }
                            }
                        }
                    }
                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 0 * 36) //Wooden chest
                    {

                        if (WorldGen.genRand.NextBool(7))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    if (Main.rand.NextBool())
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Swords.WeatheredSledgehammer>());
                                    }
                                    else
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Magic.Misc.AccursedShard>());
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 12 * 36) //Living wood chest
                    {

                        if (WorldGen.genRand.NextBool(7))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    if (Main.rand.NextBool())
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Swords.WeatheredSledgehammer>());
                                    }
                                    else
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Magic.Misc.AccursedShard>());
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 8 * 36) //Rich mahogany chest
                    {
                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    if (Main.rand.NextBool())
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Swords.WeatheredSledgehammer>());
                                    }
                                    else
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Magic.Misc.AccursedShard>());
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36) //Golden chest
                    {
                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    if (Main.rand.NextBool())
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Swords.WeatheredSledgehammer>());
                                    }
                                    else
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Magic.Misc.AccursedShard>());
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 11 * 36) //Ice chest
                    {
                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    if (Main.rand.NextBool())
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Swords.WeatheredSledgehammer>());
                                    }
                                    else
                                    {
                                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Magic.Misc.AccursedShard>());
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
