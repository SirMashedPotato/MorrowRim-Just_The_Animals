using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;

namespace MorrowRim
{
    public class IncidentWorker_SiltStrider : IncidentWorker
    {
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms) || !ModSettings_Utility.MorrowRim_SettingEnableSiltStriderEvent())
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(ThingDefOf.MorrowRim_SiltStrider) && this.TryFindEntryCell(map, out intVec);
		}

		//
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindEntryCell(map, out intVec))
			{
				return false;
			}
			Verse.PawnKindDef siltStrider = PawnKindDefOf.MorrowRim_SiltStrider;
			int num = GenMath.RoundRandom(StorytellerUtility.DefaultThreatPointsNow(map) / siltStrider.combatPower);
			int max = Rand.RangeInclusive(3, 6);
			num = Mathf.Clamp(num, 1, max);
			int num2 = Rand.RangeInclusive(90000, 150000);
			IntVec3 invalid = IntVec3.Invalid;
			if (!RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(intVec, map, 10f, out invalid))
			{
				invalid = IntVec3.Invalid;
			}
			Pawn pawn = null;
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				pawn = PawnGenerator.GeneratePawn(siltStrider, null);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
				pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num2;
				if (invalid.IsValid)
				{
					pawn.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(invalid, map, 10, null);
				}
			}
			Find.LetterStack.ReceiveLetter("LetterLabelSiltStriderPasses".Translate(siltStrider.label).CapitalizeFirst(), "LetterSiltStriderPasses".Translate(siltStrider.label), LetterDefOf.PositiveEvent, pawn, null, null);
			return true;
		}
		//

		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f, false, null);
		}
	}
}
