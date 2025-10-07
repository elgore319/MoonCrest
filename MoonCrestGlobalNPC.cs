using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoonCrest
{
	public class MoonCrestGlobalNPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			// King Slime drops Gelatinous Crystal
			if (npc.type == NPCID.KingSlime)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.GelatinousCrystal>(), 1, 1, 1));
			}

			// Queen Bee drops Stinger of Life
			if (npc.type == NPCID.QueenBee)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.StingerOfLife>(), 1, 1, 1));
			}
		}

		public override void OnKill(NPC npc)
		{
			// Prevent Wall of Flesh from enabling hardmode
			if (npc.type == NPCID.WallofFlesh)
			{
				// Cancel hardmode activation
				Main.hardMode = false;
				
				// Custom message
				if (Main.netMode != NetmodeID.Server)
				{
					Main.NewText("The Wall of Flesh crumbles, but something greater stirs in the void...", 255, 150, 150);
					Main.NewText("The ancient barriers weaken. Prepare for what comes next.", 200, 100, 255);
				}
			}
		}

		public override void SetStaticDefaults()
		{
			// Rename vanilla bosses
			NPCID.Sets.BossHeadTextures[NPCID.SkeletronHead] = ModContent.Request<Texture2D>("MoonCrest/Content/NPCs/ReanimatedBones_Head_Boss").Value;
			
			// We'll handle the Eater of Worlds renaming when we create the custom boss
		}

		public override void SetDisplayName(ref string displayName)
		{
			// Rename Skeletron to "Re-animated Bones"
			if (NPC.type == NPCID.SkeletronHead)
			{
				displayName = "Re-animated Bones";
			}
		}
	}
}