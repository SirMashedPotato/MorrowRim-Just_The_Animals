using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class KwamaJobGiver_HiveDefense : JobGiver_AIFightEnemies
    {
		// Token: 0x06002D6B RID: 11627 RVA: 0x000FF304 File Offset: 0x000FD504
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			KwamaNest hive = pawn.mindState.duty.focus.Thing as KwamaNest;
			if (hive != null && hive.Spawned)
			{
				return hive.Position;
			}
			return pawn.Position;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000FF344 File Offset: 0x000FD544
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000FF356 File Offset: 0x000FD556
		protected override Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(enemyTarget);
			job.attackDoorIfTargetLost = true;
			return job;
		}
	}
}