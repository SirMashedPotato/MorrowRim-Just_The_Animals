using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;

namespace MorrowRim
{
    public class IncidentWorker_CliffRacerSwarm_Manhunter : IncidentWorker
    {
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms) || !ModSettings_Utility.MorrowRim_SettingEnableCliffRacerEvents())
			{
				return false;
			}
			Map map = (Map)parms.target;
			PawnKindDef pawnKindDef;	//overwrite with cliff racer later
			IntVec3 intVec;
			return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKindDef) && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef pawnKind = PawnKindDefOf.MorrowRim_CliffRacer;
			if ((pawnKind == null && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKind)) || ManhunterPackIncidentUtility.GetAnimalsCount(pawnKind, parms.points) == 0)
			{
				return false;
			}
			IntVec3 spawnCenter = parms.spawnCenter;
			if (!spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out spawnCenter, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals(pawnKind, map.Tile, parms.points * 1f);
			Rot4 rot = Rot4.FromAngleFlat((map.Center - spawnCenter).AngleFlat);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spawnCenter, map, 10, null);
				QuestUtility.AddQuestTag(GenSpawn.Spawn(pawn, loc, map, rot, WipeMode.Vanish, false), parms.questTag);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
				pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(300000, 600000);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelCliffRacerSwarmManhunter".Translate(), "LetterCliffRacerSwarmManhunter".Translate(), LetterDefOf.ThreatBig, list[0], null, null);
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
			return true;
		}

		private const float PointsFactor = 3f;

		private const int AnimalsStayDurationMin = 300000;

		private const int AnimalsStayDurationMax = 600000;
	}
}
