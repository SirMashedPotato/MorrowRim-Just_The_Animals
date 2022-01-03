using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class GenStep_KwamaNest : GenStep
	{
		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003D3F RID: 15679 RVA: 0x00141D23 File Offset: 0x0013FF23
		public override int SeedPart
		{
			get
			{
				return 349641510;
			}
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x00141D2C File Offset: 0x0013FF2C
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.Storyteller.difficulty.allowCaveHives)
			{
				return;
			}
			MapGenFloatGrid caves = MapGenerator.Caves;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			float num = 0.7f;
			int num2 = 0;
			this.rockCells.Clear();
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (elevation[intVec] > num)
				{
					this.rockCells.Add(intVec);
				}
				if (caves[intVec] > 0f)
				{
					num2++;
				}
			}
			List<IntVec3> list = (from c in map.AllCells
								  where map.thingGrid.ThingsAt(c).Any((Thing thing) => thing.Faction != null)
								  select c).ToList<IntVec3>();
			GenMorphology.Dilate(list, MinDistFromFactionBase, map, null);
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>(list);
			int num3 = GenMath.RoundRandom((float)num2 / CaveCellsPerHive);
			GenMorphology.Erode(this.rockCells, MinDistToOpenSpace, map, null);
			this.possibleSpawnCells.Clear();
			for (int i = 0; i < this.rockCells.Count; i++)
			{
				if (caves[this.rockCells[i]] > 0f && !hashSet.Contains(this.rockCells[i]))
				{
					this.possibleSpawnCells.Add(this.rockCells[i]);
				}
			}
			this.spawnedHives.Clear();
			for (int j = 0; j < num3; j++)
			{
				this.TrySpawnHive(map);
			}
			this.spawnedHives.Clear();
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x00141EE4 File Offset: 0x001400E4
		private void TrySpawnHive(Map map)
		{
			IntVec3 intVec;
			if (!this.TryFindHiveSpawnCell(map, out intVec))
			{
				return;
			}
			this.possibleSpawnCells.Remove(intVec);
			KwamaNest hive = (KwamaNest)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MorrowRim_KwamaNest, null), intVec, map, WipeMode.Vanish);
			hive.SetFaction(FactionUtility.DefaultFactionFrom(FactionDefOf.MorrowRim_Kwama), null);
			hive.PawnSpawner.aggressive = false;

			(from x in hive.GetComps<CompSpawner>()
			 where x.PropsSpawner.thingToSpawn == ThingDefOf.MorrowRim_KwamaEggSac
			 select x).First<CompSpawner>().TryDoSpawn();

			hive.PawnSpawner.SpawnPawnsUntilPoints(Rand.Range(200f, 500f));
			hive.PawnSpawner.canSpawnPawns = true;
			hive.GetComp<CompSpawnerKwamaNest>().canSpawnHives = true;
			this.spawnedHives.Add(hive);
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00141FB0 File Offset: 0x001401B0
		private bool TryFindHiveSpawnCell(Map map, out IntVec3 spawnCell)
		{
			float num = -1f;
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 3; i++)
			{
				if (!possibleSpawnCells.Where((IntVec3 x) => x.Standable(map) && x.GetFirstItem(map) == null && x.GetFirstBuilding(map) == null && x.GetFirstPawn(map) == null).TryRandomElement(out IntVec3 intVec2))
				{
					break;
				}

				float num2 = -1f;
				for (int j = 0; j < this.spawnedHives.Count; j++)
				{
					float num3 = (float)intVec2.DistanceToSquared(this.spawnedHives[j].Position);
					if (num2 < 0f || num3 < num2)
					{
						num2 = num3;
					}
				}
				if (!intVec.IsValid || num2 > num)
				{
					intVec = intVec2;
					num = num2;
				}
			}
			spawnCell = intVec;
			return spawnCell.IsValid;
		}

		// Token: 0x040023AD RID: 9133
		private List<IntVec3> rockCells = new List<IntVec3>();

		// Token: 0x040023AE RID: 9134
		private List<IntVec3> possibleSpawnCells = new List<IntVec3>();

		// Token: 0x040023AF RID: 9135
		private List<KwamaNest> spawnedHives = new List<KwamaNest>();

		// Token: 0x040023B0 RID: 9136
		private const int MinDistToOpenSpace = 10;

		// Token: 0x040023B1 RID: 9137
		private const int MinDistFromFactionBase = 50;

		// Token: 0x040023B2 RID: 9138
		private const float CaveCellsPerHive = 1000f;
	}
}
