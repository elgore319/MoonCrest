using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoonCrest.Content.Items
{
	public class CthulhuHeart : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Pink;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.consumable = false;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName and Tooltip will be set in localization
		}

		public override bool? UseItem(Player player)
		{
			// Display lore when used
			if (Main.netMode != NetmodeID.Server)
			{
				Main.NewText("The heart of the ancient one pulses with cosmic power...", 255, 100, 255);
				Main.NewText("You sense that this is connected to something far greater.", 200, 150, 255);
			}
			return true;
		}

		public override void UpdateInventory(Player player)
		{
			// Passive effects while in inventory
			player.GetModPlayer<MoonCrestPlayer>().cthulhuHeart = true;
		}
	}
}