using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MoonCrest
{
	public class MoonCrestSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			// Called after all mods have loaded their content
		}

		public override void PostUpdateWorld()
		{
			// Called every frame after the world updates
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			// Force both world evils to generate for lore purposes
			int evilBiomeIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Corruption"));
			if (evilBiomeIndex != -1)
			{
				tasks.Insert(evilBiomeIndex + 1, new PassLegacy("MoonCrest: Dual Evil Biomes", GenerateBothEvils));
			}
		}

		private void GenerateBothEvils(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Awakening ancient evils...";
			
			// If only one evil exists, generate the other one on the opposite side
			bool hasCorruption = WorldGen.crimson == false;
			bool hasCrimson = WorldGen.crimson == true;

			if (hasCorruption && !hasCrimson)
			{
				// Generate Crimson on the other side
				GenerateSecondaryEvil(true); // true for crimson
			}
			else if (hasCrimson && !hasCorruption)
			{
				// Generate Corruption on the other side
				GenerateSecondaryEvil(false); // false for corruption
			}
		}

		private void GenerateSecondaryEvil(bool generateCrimson)
		{
			// This is a simplified version - in practice you'd want more sophisticated generation
			int worldCenter = Main.maxTilesX / 2;
			int evilStart = generateCrimson ? worldCenter + Main.maxTilesX / 4 : worldCenter - Main.maxTilesX / 4;
			
			// Generate a small secondary evil biome
			for (int i = evilStart - 50; i < evilStart + 50; i++)
			{
				for (int j = (int)Main.worldSurface; j < (int)Main.worldSurface + 100; j++)
				{
					if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == TileID.Stone)
					{
						if (generateCrimson)
						{
							WorldGen.PlaceTile(i, j, TileID.Crimstone, true, true);
						}
						else
						{
							WorldGen.PlaceTile(i, j, TileID.Ebonstone, true, true);
						}
					}
				}
			}
		}
	}
}