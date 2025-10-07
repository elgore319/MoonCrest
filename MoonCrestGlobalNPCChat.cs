using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoonCrest
{
	public class MoonCrestGlobalNPCChat : GlobalNPC
	{
		public override void GetChat(NPC npc, ref string chat)
		{
			if (npc.type == NPCID.Guide)
			{
				// Enhanced Guide dialogue based on progression
				chat = GetGuideDialogue();
			}
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			if (NPC.type == NPCID.Guide)
			{
				button = Language.GetTextValue("LegacyInterface.28"); // "Help"
				button2 = "Ancient Knowledge";
			}
		}

		public override void OnChatButtonClicked(NPC npc, bool firstButton)
		{
			if (npc.type == NPCID.Guide)
			{
				if (!firstButton) // "Ancient Knowledge" button
				{
					string loreText = GetGuideLore();
					Main.npcChatText = loreText;
				}
			}
		}

		private string GetGuideDialogue()
		{
			Player player = Main.LocalPlayer;
			
			// Check if player has Gelatinous Crystal
			bool hasGelatinousCrystal = player.HasItem(ModContent.ItemType<Content.Items.GelatinousCrystal>());
			bool hasStingerOfLife = player.HasItem(ModContent.ItemType<Content.Items.StingerOfLife>());
			
			List<string> dialogues = new List<string>();

			// Early game dialogue
			if (!NPC.downedBoss1) // Before King Slime
			{
				dialogues.Add("The stars whisper of ancient powers stirring beneath the earth...");
				dialogues.Add("Something feels different about this world. The very air carries whispers of old magic.");
				dialogues.Add("I sense a great darkness approaching. You must prepare yourself for what's to come.");
			}
			else if (hasGelatinousCrystal && !NPC.downedBoss2) // After King Slime, before other bosses
			{
				dialogues.Add("That crystal you carry... it pulses with the same energy I felt in my dreams.");
				dialogues.Add("The Gelatinous Crystal is just the beginning. Ancient forces are awakening.");
				dialogues.Add("I've seen visions of a great eye watching from the depths. Beware the corruption that spreads.");
			}
			else if (DownedBossSystem.downedSpineOfCthulhu) // After Spine of Cthulhu
			{
				dialogues.Add("The Spine of Cthulhu has fallen, but its master still slumbers. This is only the beginning.");
				dialogues.Add("You've proven yourself against one of the old one's servants. But greater challenges await.");
				dialogues.Add("The bones of the ancient guardian lie shattered, yet I feel no peace. Something darker approaches.");
			}
			else if (hasStingerOfLife) // After Queen Bee
			{
				dialogues.Add("The Queen's essence flows through that stinger. Life and death intertwined... fascinating.");
				dialogues.Add("You carry the power of life itself. Use it wisely, for darker times are coming.");
				dialogues.Add("The jungle's guardian has blessed you with her power. You'll need it for what lies ahead.");
			}
			else if (NPC.downedBoss3 && !Main.downedWallofFlesh) // After Skeletron, before Wall of Flesh
			{
				dialogues.Add("The Re-animated Bones have crumbled, but their master's influence grows stronger.");
				dialogues.Add("You've faced death itself and emerged victorious. The old powers take notice.");
				dialogues.Add("The underworld calls to you. Face the wall of ancient flesh to break the first seal.");
			}
			else if (Main.downedWallofFlesh && !DownedBossSystem.downedCthulhu) // After Wall of Flesh, before Cthulhu
			{
				dialogues.Add("The Wall of Flesh has fallen, but its death was merely the first step.");
				dialogues.Add("I sense something ancient stirring. The stars are aligning for a terrible awakening.");
				dialogues.Add("The Cosmic Eye... if you possess one, beware. Some doors should never be opened.");
				dialogues.Add("The barrier between worlds weakens. Something vast approaches from beyond the stars.");
			}
			else if (DownedBossSystem.downedCthulhu) // After Cthulhu
			{
				dialogues.Add("You've done the impossible - defeated Cthulhu itself. But its death has changed everything.");
				dialogues.Add("The world has entered a new age. Darker creatures now walk the earth.");
				dialogues.Add("Cthulhu's heart still beats with cosmic power. Guard it well, for others will seek it.");
			}
			else // Default enhanced dialogue
			{
				dialogues.Add("The world trembles with ancient power. Can you feel it in the air?");
				dialogues.Add("My dreams are filled with visions of a great eye watching from the void.");
				dialogues.Add("Something stirs in the depths. The old gods are not as dead as we hoped.");
			}

			return dialogues[Main.rand.Next(dialogues.Count)];
		}

		private string GetGuideLore()
		{
			List<string> loreTexts = new List<string>();

			if (!NPC.downedBoss1)
			{
				loreTexts.Add("Long ago, this world was whole. But a great catastrophe shattered reality itself, " +
					"creating the dual corruptions you see today. The Crimson and Corruption are not natural - " +
					"they are scars left by something far more terrible.");
			}
			else if (DownedBossSystem.downedSpineOfCthulhu)
			{
				loreTexts.Add("The Spine of Cthulhu was once part of a much greater entity. Legend speaks of " +
					"a cosmic horror that fell from the stars, its body scattered across dimensions. " +
					"Each piece that remains seeks to reunite with its master.");
			}
			else if (NPC.downedBoss3)
			{
				loreTexts.Add("The Re-animated Bones were once a great guardian, tasked with protecting " +
					"the secrets within the dungeon. But death could not contain the dark knowledge within, " +
					"and so the bones walk again, hollow and purposeless.");
			}
			else
			{
				loreTexts.Add("The crystals and artifacts you collect are not mere treasures. Each one " +
					"carries fragments of memory from the time before the great sundering. " +
					"Gather them wisely - they may be the key to understanding what's coming.");
			}

			return loreTexts[Main.rand.Next(loreTexts.Count)];
		}
	}
}