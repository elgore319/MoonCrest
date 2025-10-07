using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoonCrest.Content.Items
{
	public class StingerOfLife : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Orange;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName and Tooltip will be set in localization
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// Grant life regeneration bonus
			player.lifeRegen += 2;
			
			// Grant immunity to poisoned debuff
			player.buffImmune[BuffID.Poisoned] = true;
			
			// Small defense bonus
			player.statDefense += 3;
			
			// Chance to inflict poison on attackers
			player.GetModPlayer<MoonCrestPlayer>().stingerOfLife = true;
		}
	}
}