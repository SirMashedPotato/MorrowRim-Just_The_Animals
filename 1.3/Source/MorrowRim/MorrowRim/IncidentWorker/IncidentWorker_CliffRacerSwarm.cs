using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;

namespace MorrowRim
{
    public class IncidentWorker_CliffRacerSwarm : IncidentWorker
    {
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms) || !ModSettings_Utility.MorrowRim_SettingEnableCliffRacerEvents())
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef cliffRacer = PawnKindDefOf.MorrowRim_CliffRacer;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			float freeColonistsCount = (float)map.mapPawns.FreeColonistsCount;
			float randomInRange = IncidentWorker_CliffRacerSwarm.CountPerColonistRange.RandomInRange;
			int num = Mathf.Clamp(GenMath.RoundRandom(freeColonistsCount * randomInRange), 1, 10);
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				((Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(cliffRacer, null), loc, map, WipeMode.Vanish)).needs.food.CurLevelPercentage = 1f;
			}
			//base.SendStandardLetter("LetterLabelCliffRacerSwarm".Translate(), "LetterCliffRacerSwarm".Translate(), LetterDefOf.ThreatSmall, parms, new TargetInfo(intVec, map, false), null);
			Find.LetterStack.ReceiveLetter("LetterLabelCliffRacerSwarm".Translate(), "LetterCliffRacerSwarm".Translate(), LetterDefOf.ThreatSmall, new TargetInfo(intVec, map, false), null, null);
			return true;
		}

		private static readonly FloatRange CountPerColonistRange = new FloatRange(1f, 1.5f);
		private const int MinCount = 3;
		private const int MaxCount = 20;
	}
}
