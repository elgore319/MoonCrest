using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoonCrest.Content.NPCs
{
	[AutoloadBossHead]
	public class Cthulhu : ModNPC
	{
		private int attackTimer = 0;
		private int attackPhase = 0;
		private bool secondPhase = false;

		public override void SetStaticDefaults()
		{
			// DisplayName will be set in localization
			Main.npcFrameCount[Type] = 1;
			
			NPCID.Sets.BossHeadTextures[Type] = ModContent.Request<Texture2D>("MoonCrest/Content/NPCs/Cthulhu_Head_Boss");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			
			// This boss is immune to all debuffs like Moon Lord
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 100;
			NPC.height = 100;
			NPC.damage = 60;
			NPC.defense = 25;
			NPC.lifeMax = 12000; // Between Wall of Flesh and early hardmode bosses
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(gold: 15);
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1; // Custom AI
			NPC.boss = true;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			
			if (!Main.dedServ)
			{
				Music = MusicID.Boss2;
			}
		}

		public override void AI()
		{
			// Check if second phase should activate
			if (NPC.life < NPC.lifeMax / 2 && !secondPhase)
			{
				secondPhase = true;
				attackPhase = 0;
				attackTimer = 0;
				
				// Announcement for phase 2
				if (Main.netMode != NetmodeID.Server)
				{
					Main.NewText("Cthulhu's true power awakens!", 255, 0, 0);
				}
			}

			// Target the closest player
			Player target = Main.player[NPC.target];
			if (!target.active || target.dead)
			{
				NPC.TargetClosest(false);
				target = Main.player[NPC.target];
				if (!target.active || target.dead)
				{
					NPC.velocity.Y -= 0.04f;
					if (NPC.timeLeft > 10)
						NPC.timeLeft = 10;
					return;
				}
			}

			attackTimer++;

			// Phase 1 AI
			if (!secondPhase)
			{
				Phase1AI(target);
			}
			else
			{
				Phase2AI(target);
			}

			// Rotation towards player
			float rotation = (float)Math.Atan2(target.Center.Y - NPC.Center.Y, target.Center.X - NPC.Center.X);
			NPC.rotation = rotation + MathHelper.PiOver2;
		}

		private void Phase1AI(Player target)
		{
			// Hover movement
			Vector2 targetPosition = target.Center - new Vector2(0, 200);
			Vector2 direction = targetPosition - NPC.Center;
			direction.Normalize();
			NPC.velocity = Vector2.Lerp(NPC.velocity, direction * 3f, 0.02f);

			// Attack patterns
			if (attackTimer >= 120) // Attack every 2 seconds
			{
				switch (attackPhase)
				{
					case 0: // Laser attack
						LaserAttack(target);
						break;
					case 1: // Projectile spam
						ProjectileSpam(target);
						break;
					case 2: // Summon minions
						SummonMinions();
						break;
				}

				attackPhase = (attackPhase + 1) % 3;
				attackTimer = 0;
			}
		}

		private void Phase2AI(Player target)
		{
			// More aggressive movement
			Vector2 targetPosition = target.Center - new Vector2(0, 150);
			Vector2 direction = targetPosition - NPC.Center;
			direction.Normalize();
			NPC.velocity = Vector2.Lerp(NPC.velocity, direction * 5f, 0.03f);

			// Faster, more dangerous attacks
			if (attackTimer >= 80) // Attack every 1.33 seconds
			{
				switch (attackPhase)
				{
					case 0:
						LaserAttack(target);
						LaserAttack(target); // Double laser
						break;
					case 1:
						ProjectileSpam(target);
						ProjectileSpam(target); // Double spam
						break;
					case 2:
						SummonMinions();
						break;
					case 3: // New attack for phase 2
						TentacleAttack(target);
						break;
				}

				attackPhase = (attackPhase + 1) % 4;
				attackTimer = 0;
			}
		}

		private void LaserAttack(Player target)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 8f, 
					ProjectileID.EyeLaser, 25, 0f, Main.myPlayer);
				
				SoundEngine.PlaySound(SoundID.Item33, NPC.position);
			}
		}

		private void ProjectileSpam(Player target)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < 5; i++)
				{
					Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
					direction = direction.RotatedBy(MathHelper.Lerp(-0.5f, 0.5f, i / 4f));
					
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 6f,
						ProjectileID.CultistBossFireBall, 20, 0f, Main.myPlayer);
				}
				
				SoundEngine.PlaySound(SoundID.Item20, NPC.position);
			}
		}

		private void SummonMinions()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				// Summon 2-3 Eyes of Cthulhu as minions
				for (int i = 0; i < Main.rand.Next(2, 4); i++)
				{
					Vector2 spawnPos = NPC.Center + new Vector2(Main.rand.Next(-400, 400), Main.rand.Next(-200, 200));
					NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnPos.X, (int)spawnPos.Y, NPCID.EyeofCthulhu);
				}
				
				SoundEngine.PlaySound(SoundID.Roar, NPC.position);
			}
		}

		private void TentacleAttack(Player target)
		{
			// Phase 2 exclusive attack - spawns projectiles in a circle
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < 8; i++)
				{
					float angle = MathHelper.TwoPi * i / 8f;
					Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 4f,
						ProjectileID.DeathLaser, 30, 0f, Main.myPlayer);
				}
				
				SoundEngine.PlaySound(SoundID.Item62, NPC.position);
			}
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Drop items to enable hardmode progression
			npcLoot.Add(ItemDropRule.Common(ItemID.HallowedBar, 1, 15, 25));
			npcLoot.Add(ItemDropRule.Common(ItemID.SoulofFlight, 1, 20, 30));
			
			// Custom lore drop
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.CthulhuHeart>(), 1, 1, 1));
		}

		public override void OnKill()
		{
			// Enable hardmode (this is the true hardmode trigger)
			Main.hardMode = true;
			
			// Lore message
			if (Main.netMode != NetmodeID.Server)
			{
				Main.NewText("The ancient one falls, but its death unleashes greater horrors upon the world...", 255, 100, 100);
				Main.NewText("The world has entered Hardmode!", 50, 255, 130);
			}
			
			// Mark as defeated
			NPC.SetEventFlagCleared(ref DownedBossSystem.downedCthulhu, -1);
			
			// Spawn hardmode ores and trigger hardmode changes
			WorldGen.StartHardmode();
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}
	}
}