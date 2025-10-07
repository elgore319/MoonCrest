using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoonCrest.Content.Items
{
	public class GelatinousCrystal : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.consumable = false;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName and Tooltip will be set in localization
		}

		public override bool? UseItem(Player player)
		{
			// Add lore text when used
			if (Main.netMode != NetmodeID.Server)
			{
				Main.NewText("The crystal pulses with an otherworldly energy...", 150, 255, 150);
			}
			return true;
		}
	}
}