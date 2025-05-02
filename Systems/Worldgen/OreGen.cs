using Terraria.WorldBuilding;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Wisplantern.Items.Materials;
using Wisplantern.Items.Placeable.Blocks;

namespace Wisplantern.Systems.Worldgen
{
    class OreGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            tasks.Insert(genIndex + 1, new PassLegacy("Wisplantern Shinies", delegate (GenerationProgress progress, GameConfiguration config)
            {
                progress.Message = "Shinier shinies";

                Hyperstone();
                Pyrite();
                Pandemonium();
            }));
        }

        public void Hyperstone()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 2); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(8, 10), WorldGen.genRand.Next(1, 3), ModContent.TileType<Tiles.Hyperstone>(), false, 0f, 0f, false, true);
            }
        }

        public void Pyrite()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 4); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)20, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(2, 4), ModContent.TileType<Tiles.Pyrite>(), false, 0f, 0f, false, true);
            }

            if (Wisplantern.generateVolcanicCaves)
            {
                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 1.5f); k++)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, (int)Main.maxTilesY - 300);

                    if (IgneousCaveGen.ShouldConvertTile(x, y, 10))
                    {
                        WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(12, 16), WorldGen.genRand.Next(2, 4), ModContent.TileType<Tiles.Pyrite>(), false, 0f, 0f, false, true);
                    }
                }
            }
        }

        public override void PostAddRecipes()
        {
            int n = Recipe.numRecipes;
            List<(Recipe, Recipe)> addedRecipes = new();
            for (int i = 0; i < n; i++)
            {
                Recipe baseRecipe = Main.recipe[i];

                if (baseRecipe.HasTile(TileID.Furnaces) || baseRecipe.HasTile(TileID.Hellforge) || baseRecipe.HasTile(TileID.GlassKiln) || baseRecipe.HasTile(TileID.LihzahrdFurnace) || baseRecipe.HasTile(TileID.AdamantiteForge))
                {
                    List<Item> newRequiredItems = new();
                    foreach (Item item in baseRecipe.requiredItem)
                    {
                        if (item.stack > 1)
                        {
                            newRequiredItems.Add(new Item(item.type, item.stack / 2));
                        }
                        else
                        {
                            newRequiredItems.Add(new Item(item.type, item.stack));
                        }
                    }

                    if (newRequiredItems.Count > 0)
                    {
                        int extra = 0;

                        if (baseRecipe.createItem.type == ItemID.Glass
                            || baseRecipe.createItem.type == ItemID.GrayBrick
                            || baseRecipe.createItem.type == ItemID.IridescentBrick
                            || baseRecipe.createItem.type == ItemID.SandstoneBrick
                            || baseRecipe.createItem.type == ItemID.SmoothSandstone
                            || baseRecipe.createItem.type == ItemID.PearlstoneBrick
                            || baseRecipe.createItem.type == ItemID.EbonstoneBrick
                            || baseRecipe.createItem.type == ItemID.CrimstoneBrick
                            || baseRecipe.createItem.type == ItemID.MudstoneBlock
                            || baseRecipe.createItem.type == ItemID.RedBrick
                            || baseRecipe.createItem.type == ModContent.ItemType<BlackBrick>())
                        {
                            extra = 1;
                        }

                        Recipe recipe = Recipe.Create(baseRecipe.createItem.type, (int)(baseRecipe.createItem.stack * 1.5f) + extra);
                        foreach (Item item in newRequiredItems)
                        {
                            recipe.AddIngredient(item.type, item.stack);
                        }
                        recipe.AddIngredient<Pyrite>();
                        foreach (int t in baseRecipe.requiredTile)
                        {
                            recipe.AddTile(t);
                        }
                        recipe.Register();
                        addedRecipes.Add((recipe, baseRecipe));
                    }
                }
            }

            foreach ((Recipe, Recipe) recipe in addedRecipes)
            {
                recipe.Item1.SortAfter(recipe.Item2);
            }
        }

        public void Pandemonium()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 10); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(12, 16), WorldGen.genRand.Next(2, 4), ModContent.TileType<Tiles.PandemoniumOre>(), false, 0f, 0f, false, true);
            }
        }

    }
}
