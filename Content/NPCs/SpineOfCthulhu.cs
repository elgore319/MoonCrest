using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoonCrest.Content.NPCs
{
	public class SpineOfCthulhu : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName will be set in localization
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.EaterofWorldsHead];
			
			NPCID.Sets.BossHeadTextures[Type] = ModContent.Request<Texture2D>("MoonCrest/Content/NPCs/SpineOfCthulhu_Head_Boss");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void SetDefaults()
		{
			// Base stats similar to Eater of Worlds Head
			NPC.width = 30;
			NPC.height = 30;
			NPC.damage = 22;
			NPC.defense = 2;
			NPC.lifeMax = 65;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 150f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 6; // Worm AI
			NPC.boss = true;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = true;
			NPC.netAlways = true;
			
			// Set as boss
			if (!Main.dedServ)
			{
				Music = MusicID.Boss1;
			}
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.LesserHealingPotion;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Drop same items as Eater of Worlds but with our lore context
			npcLoot.Add(ItemDropRule.Common(ItemID.ShadowScale, 1, 4, 8));
			npcLoot.Add(ItemDropRule.Common(ItemID.DemoniteOre, 1, 15, 30));
		}

		public override void OnKill()
		{
			// Lore message when defeated
			if (Main.netMode != NetmodeID.Server)
			{
				Main.NewText("The ancient spine crumbles, its dark purpose unfulfilled...", 200, 100, 255);
			}
			
			// Mark as defeated for world progression
			NPC.SetEventFlagCleared(ref DownedBossSystem.downedSpineOfCthulhu, -1);
		}
	}
}