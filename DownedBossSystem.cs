using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MoonCrest
{
	public class DownedBossSystem : ModSystem
	{
		public static bool downedSpineOfCthulhu = false;
		public static bool downedReanimatedBones = false;
		public static bool downedCthulhu = false;

		public override void ClearWorld()
		{
			downedSpineOfCthulhu = false;
			downedReanimatedBones = false;
			downedCthulhu = false;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			if (downedSpineOfCthulhu)
				tag["downedSpineOfCthulhu"] = true;
			if (downedReanimatedBones)
				tag["downedReanimatedBones"] = true;
			if (downedCthulhu)
				tag["downedCthulhu"] = true;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			downedSpineOfCthulhu = tag.ContainsKey("downedSpineOfCthulhu");
			downedReanimatedBones = tag.ContainsKey("downedReanimatedBones");
			downedCthulhu = tag.ContainsKey("downedCthulhu");
		}

		public override void NetSend(BinaryWriter writer)
		{
			var flags = new BitsByte();
			flags[0] = downedSpineOfCthulhu;
			flags[1] = downedReanimatedBones;
			flags[2] = downedCthulhu;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedSpineOfCthulhu = flags[0];
			downedReanimatedBones = flags[1];
			downedCthulhu = flags[2];
		}
	}
}