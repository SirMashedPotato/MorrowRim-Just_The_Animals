using System;
using Verse;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class CompProperties_SpawnerKwamaNest : CompProperties
    {
		public CompProperties_SpawnerKwamaNest()
		{
			this.compClass = typeof(CompSpawnerKwamaNest);
		}

		// Token: 0x04002D3F RID: 11583
		public float HiveSpawnPreferredMinDist = 5f;

		// Token: 0x04002D40 RID: 11584
		public float HiveSpawnRadius = 10f;

		// Token: 0x04002D41 RID: 11585
		public FloatRange HiveSpawnIntervalDays = new FloatRange(15f, 30);

		// Token: 0x04002D42 RID: 11586
		public SimpleCurve ReproduceRateFactorFromNearbyHiveCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(7f, 0.35f),
				true
			}
		};
	}
}
