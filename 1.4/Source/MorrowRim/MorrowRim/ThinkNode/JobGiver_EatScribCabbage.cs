using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace MorrowRim
{
    class JobGiver_EatScribCabbage : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
			if (pawn.Downed)
			{
				return null;
			}
			bool validator(Thing t) => t.def.category == ThingCategory.Item && t.def == ThingDefOf.MorrowRim_RawScribCabbage && t.IngestibleNow && pawn.RaceProps.CanEverEat(t) && pawn.CanReserve(t, 1, -1, null, false);
            Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 10f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing == null)
			{
				return null;
			}
			return JobMaker.MakeJob(RimWorld.JobDefOf.Ingest, thing);
		}
	}
}
