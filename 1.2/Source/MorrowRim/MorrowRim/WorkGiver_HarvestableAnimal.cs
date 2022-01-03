using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    class WorkGiver_HarvestableAnimal : WorkGiver_GatherAnimalBodyResources
    {
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.MorrowRim_HarvestAnimalGrowth;
			}
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000F7970 File Offset: 0x000F5B70
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<MorrowRim.Comp_Harvestable>();
		}
	}
}
