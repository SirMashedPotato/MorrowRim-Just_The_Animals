using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
	public class CompProperties_Harvestable : CompProperties
	{
		public CompProperties_Harvestable()
		{
			this.compClass = typeof(Comp_Harvestable);
		}

		public int harvestIntervalDays;

		public int harvestAmount = 1;

		public ThingDef harvestDef;
	}
}
