using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoonCrest.Content.Items
{
	public class CosmicEye : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 20;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = ItemRarityID.Blue;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.consumable = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName and Tooltip will be set in localization
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // Same as other boss summons
		}

		public override bool CanUseItem(Player player)
		{
			// Can only be used if Wall of Flesh is defeated and Cthulhu hasn't been defeated yet
			return Main.hardMode == false && Main.downedWallofFlesh && !DownedBossSystem.downedCthulhu;
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				// Check if boss is already active
				if (NPC.AnyNPCs(ModContent.NPCType<Content.NPCs.Cthulhu>()))
				{
					return false;
				}

				// Spawn the boss
				SoundEngine.PlaySound(SoundID.Roar, player.position);
				
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 spawnPos = player.Center - new Vector2(0, 400);
					int boss = NPC.NewNPC(player.GetSource_ItemUse(Item), (int)spawnPos.X, (int)spawnPos.Y, 
						ModContent.NPCType<Content.NPCs.Cthulhu>());
					
					if (Main.netMode == NetmodeID.Server && boss < Main.maxNPCs)
					{
						NetMessage.SendData(MessageID.SyncNPC, number: boss);
					}
				}

				// Lore message
				if (Main.netMode != NetmodeID.Server)
				{
					Main.NewText("The ancient one stirs from its cosmic slumber...", 255, 0, 0);
				}

				return true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GelatinousCrystal>(), 1);
			recipe.AddIngredient(ItemID.GuideVoodooDoll, 1); // Wall of Flesh connection
			recipe.AddIngredient(ItemID.SuspiciousLookingEye, 2);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ItemID.SoulofLight, 15);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}