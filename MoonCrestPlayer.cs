using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoonCrest
{
	public class MoonCrestPlayer : ModPlayer
	{
		public bool stingerOfLife;
		public bool cthulhuHeart;
		
		public override void ResetEffects()
		{
			stingerOfLife = false;
			cthulhuHeart = false;
		}
		
		public override void PostUpdateMiscEffects()
		{
			// Cthulhu Heart passive effects
			if (cthulhuHeart)
			{
				// Increase max mana
				Player.statManaMax2 += 50;
				
				// Increase magic damage
				Player.GetDamage(DamageClass.Magic) += 0.1f;
				
				// Night vision
				Player.nightVision = true;
				
				// Slight life regeneration
				Player.lifeRegen += 1;
			}
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			// Stinger of Life effect - chance to poison attacker
			if (stingerOfLife && Main.rand.NextBool(4)) // 25% chance
			{
				npc.AddBuff(BuffID.Poisoned, 300); // 5 seconds
			}
		}

		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			// Stinger of Life effect for projectiles too
			if (stingerOfLife && Main.rand.NextBool(4) && proj.npcProj)
			{
				Main.npc[proj.owner].AddBuff(BuffID.Poisoned, 300);
			}
		}
	}
}