using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace MorrowRim.Kwama
{
	public class KwamaJobGiver_WanderHive : JobGiver_Wander
	{
		// Token: 0x06002D75 RID: 11637 RVA: 0x000FF552 File Offset: 0x000FD752
		public KwamaJobGiver_WanderHive()
		{
			this.wanderRadius = 7.5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000FF578 File Offset: 0x000FD778
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			KwamaNest hive = pawn.mindState.duty.focus.Thing as KwamaNest;
			if (hive == null || !hive.Spawned)
			{
				return pawn.Position;
			}
			return hive.Position;
		}
	}
}
