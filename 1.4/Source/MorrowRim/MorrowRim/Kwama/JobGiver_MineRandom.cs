using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace MorrowRim.Kwama
{
    class JobGiver_MineRandom : ThinkNode_JobGiver
	{
		// Token: 0x06002ED7 RID: 11991 RVA: 0x00107C20 File Offset: 0x00105E20
		protected override Job TryGiveJob(Pawn pawn)
		{
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return null;
			}
			for (int i = 0; i < 40; i++)
			{
				IntVec3 randomCell = region.RandomCell;
				for (int j = 0; j < 4; j++)
				{
					IntVec3 c = randomCell + GenAdj.CardinalDirections[j];
					if (c.InBounds(pawn.Map))
					{
						Building edifice = c.GetEdifice(pawn.Map);
						if (edifice != null && edifice.def.passability == Traversability.Impassable && edifice.def.IsNonResourceNaturalRock && edifice.def.size == IntVec2.One && edifice.def != RimWorld.ThingDefOf.CollapsedRocks && pawn.CanReserve(edifice, 1, -1, null, false))
						{
							Job job = JobMaker.MakeJob(RimWorld.JobDefOf.Mine, edifice);
							job.ignoreDesignations = true;
							return job;
						}
					}
				}
			}
			return null;
		}
	}
}
