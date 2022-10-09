using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace MorrowRim.Kwama
{
    class IncidentWorker_Kwama : IncidentWorker
    {
		// Token: 0x06003B70 RID: 15216 RVA: 0x001392FC File Offset: 0x001374FC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return ModSettings_Utility.MorrowRim_SettingEnableKwamaNestEmerging() && base.CanFireNowSub(parms) && 
				KwamaNestUtility.TotalSpawnedHivesCount(map) <= ModSettings_Utility.MorrowRim_SettingKwamaMaxNests() && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x00139334 File Offset: 0x00137534
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing t = KwamaNestUtility.SpawnTunnels(Mathf.Max(GenMath.RoundRandom(parms.points / 220f), 1), map, false, null);
			if(t != null)
            {
				Find.LetterStack.ReceiveLetter("LetterLabelKwamaNestEmerges".Translate().CapitalizeFirst(), "LetterKwamaNestEmerges".Translate(), LetterDefOf.NeutralEvent, t, null, null);
				Find.TickManager.slower.SignalForceNormalSpeedShort();
				return true;
			}
			return false;
		}

		// Token: 0x04002307 RID: 8967
		public const float HivePoints = 220f;
	}
}