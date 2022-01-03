using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace MorrowRim.Kwama
{
	class IncidentWorker_TrojanInfestation : IncidentWorker
	{
		// Token: 0x06003C5A RID: 15450 RVA: 0x0013ECC0 File Offset: 0x0013CEC0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && ModSettings_Utility.MorrowRim_SettingEnableKwamaTrojanInfestation() && KwamaNestUtility.TotalSpawnedHivesCount(map) > 0;
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x0013ECF8 File Offset: 0x0013CEF8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing targetNest = GetSpawnedNests(map).RandomElement();
			//Thing t = InfestationUtility.SpawnTunnels(Mathf.Max(GenMath.RoundRandom(parms.points / 220f), 1), map, false, false, null);
			spawnInsectoids(targetNest, map, parms);
			base.SendStandardLetter(parms, targetNest);
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}

		// Token: 0x04002387 RID: 9095
		public const float HivePoints = 220f;

		//gets all the nests
		public static Thing[] GetSpawnedNests(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.MorrowRim_KwamaNest).ToArray();
		}

		public void spawnInsectoids(Thing targetNest, Map map, IncidentParms parms)
		{
			IntVec3 loc = targetNest.Position;

			TunnelHiveSpawner tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(RimWorld.ThingDefOf.TunnelHiveSpawner, null);
			tunnelHiveSpawner.spawnHive = false;
			tunnelHiveSpawner.insectsPoints = Mathf.Clamp(parms.points * Rand.Range(0.3f, 0.6f), 200f, 1000f);
			tunnelHiveSpawner.spawnedByInfestationThingComp = true;
			GenSpawn.Spawn(tunnelHiveSpawner, loc, map, WipeMode.FullRefund);

			//Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(RimWorld.ThingDefOf.TunnelHiveSpawner, null), loc, map, WipeMode.FullRefund);
			//QuestUtility.AddQuestTag(thing, questTag);
			/*
			for (int i = 0; i < hiveCount - 1; i++)
			{
				loc = CompSpawnerHives.FindChildHiveLocation(thing.Position, map, RimWorld.ThingDefOf.Hive, RimWorld.ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), true, true);
				if (loc.IsValid)
				{
					thing = GenSpawn.Spawn(ThingMaker.MakeThing(RimWorld.ThingDefOf.TunnelHiveSpawner, null), loc, map, WipeMode.FullRefund);
					QuestUtility.AddQuestTag(thing, questTag);
				}
			}
			return thing;
			*/
		}
	}
}
