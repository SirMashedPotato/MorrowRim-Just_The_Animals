using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace MorrowRim
{
	class JobGiver_Insult : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.Message("Trying to make scamp insult someone");
			Pawn target = TryFindNewTarget(pawn);
			Job result;
			if (target == null)
			{
				result = null;
			}
			else
			{
				result = JobOnThing(pawn, target);
			}
			return result;
		}

		public Job JobOnThing(Pawn pawn, Pawn t, bool forced = false)
		{
			Log.Message("Insulintg: " + t);
			return JobMaker.MakeJob(RimWorld.JobDefOf.Insult, t);
		}

		//

		private Pawn TryFindNewTarget(Pawn pawn)
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, candidates, false);
			bool result = candidates.TryRandomElement(out Pawn target);
			candidates.Clear();
			Log.Message("Found target: " + target);
			return target;
		}

		private static List<Pawn> candidates = new List<Pawn>();
	}
}
