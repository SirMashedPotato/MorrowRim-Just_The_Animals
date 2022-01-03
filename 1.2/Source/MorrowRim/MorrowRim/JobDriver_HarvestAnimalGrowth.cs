using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    class JobDriver_HarvestAnimalGrowth : JobDriver_GatherAnimalBodyResources
	{
		protected override float WorkTotal
		{
			get
			{
				return 2000f;
			}
		}

		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<MorrowRim.Comp_Harvestable>();
		}
	}
}
