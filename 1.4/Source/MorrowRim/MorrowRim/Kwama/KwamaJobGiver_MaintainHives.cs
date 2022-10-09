using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class KwamaJobGiver_MaintainHives : JobGiver_AIFightEnemies
    {
		// Token: 0x06002D6F RID: 11631 RVA: 0x000FF36E File Offset: 0x000FD56E
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			KwamaJobGiver_MaintainHives jobGiver_MaintainHives = (KwamaJobGiver_MaintainHives)base.DeepCopy(resolve);
			jobGiver_MaintainHives.onlyIfDamagingState = this.onlyIfDamagingState;
			return jobGiver_MaintainHives;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000FF388 File Offset: 0x000FD588
		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			int num = 0;
			while ((float)num < CellsInScanRadius)
			{
				IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(pawn.Map) && intVec.GetRoom(pawn.Map) == room)
				{
					KwamaNest hive = (KwamaNest)pawn.Map.thingGrid.ThingAt(intVec, ThingDefOf.MorrowRim_KwamaNest);
					if (hive != null && pawn.CanReserve(hive, 1, -1, null, false))
					{
						CompMaintainable compMaintainable = hive.TryGetComp<CompMaintainable>();
						if (compMaintainable.CurStage != MaintainableStage.Healthy && (!this.onlyIfDamagingState || compMaintainable.CurStage == MaintainableStage.Damaging))
						{
							return JobMaker.MakeJob(RimWorld.JobDefOf.Maintain, hive);
						}
					}
				}
				num++;
			}
			return null;
		}

		// Token: 0x04001A0D RID: 6669
		private bool onlyIfDamagingState;

		// Token: 0x04001A0E RID: 6670
		private static readonly float CellsInScanRadius = (float)GenRadial.NumCellsInRadius(7.9f);
	}
}
