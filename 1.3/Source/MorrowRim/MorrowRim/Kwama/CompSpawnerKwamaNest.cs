using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class CompSpawnerKwamaNest : ThingComp
    {
		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06005194 RID: 20884 RVA: 0x001B423B File Offset: 0x001B243B
		private CompProperties_SpawnerKwamaNest Props
		{
			get
			{
				return (CompProperties_SpawnerKwamaNest)this.props;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x001B4248 File Offset: 0x001B2448
		private bool CanSpawnChildHive
		{
			get
			{
				return this.canSpawnHives && KwamaNestUtility.TotalSpawnedHivesCount(this.parent.Map) <= ModSettings_Utility.MorrowRim_SettingKwamaMaxNests();
			}
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x001B4268 File Offset: 0x001B2468
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x001B4274 File Offset: 0x001B2474
		public override void CompTick()
		{
			base.CompTick();
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			if ((comp == null || comp.Awake) && !this.wasActivated)
			{
				this.CalculateNextHiveSpawnTick();
				this.wasActivated = true;
			}
			if ((comp == null || comp.Awake) && Find.TickManager.TicksGame >= this.nextHiveSpawnTick)
			{
				KwamaNest t;
				if (this.TrySpawnChildHive(false, out t))
				{
					Messages.Message("MessageHiveReproduced".Translate(), t, MessageTypeDefOf.NegativeEvent, true);
					return;
				}
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x001B4308 File Offset: 0x001B2508
		public override string CompInspectStringExtra()
		{
			if (!this.canSpawnHives)
			{
				return "DormantHiveNotReproducing".Translate();
			}
			if (this.CanSpawnChildHive)
			{
				return "HiveReproducesIn".Translate() + ": " + (this.nextHiveSpawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true);
			}
			return null;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x001B4370 File Offset: 0x001B2570
		public void CalculateNextHiveSpawnTick()
		{
			Room room = this.parent.GetRoom(RegionType.Set_Passable);
			int num = 0;
			int num2 = GenRadial.NumCellsInRadius(9f);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && intVec.GetRoom(this.parent.Map) == room)
				{
					if (intVec.GetThingList(this.parent.Map).Any((Thing t) => t is Hive))
					{
						num++;
					}
				}
			}
			float num3 = this.Props.ReproduceRateFactorFromNearbyHiveCountCurve.Evaluate((float)num);
			this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)(this.Props.HiveSpawnIntervalDays.RandomInRange * 60000f / (num3 * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x001B4480 File Offset: 0x001B2680
		public bool TrySpawnChildHive(bool ignoreRoofedRequirement, out KwamaNest newHive)
		{
			if (!this.CanSpawnChildHive)
			{
				newHive = null;
				return false;
			}
			IntVec3 loc = FindChildHiveLocation(this.parent.Position, this.parent.Map, this.parent.def, Props, ignoreRoofedRequirement, false);
			if (!loc.IsValid)
			{
				newHive = null;
				return false;
			}
			newHive = (KwamaNest)ThingMaker.MakeThing(this.parent.def, null);
			if (newHive.Faction != this.parent.Faction)
			{
				newHive.SetFaction(this.parent.Faction, null);
			}
			KwamaNest hive = this.parent as KwamaNest;
			if (hive != null)
			{
				if (hive.CompDormant.Awake)
				{
					newHive.CompDormant.WakeUp();
				}
				newHive.questTags = hive.questTags;
			}
			GenSpawn.Spawn(newHive, loc, this.parent.Map, WipeMode.FullRefund);
			this.CalculateNextHiveSpawnTick();
			return true;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x001B4568 File Offset: 0x001B2768
		public static IntVec3 FindChildHiveLocation(IntVec3 pos, Map map, ThingDef parentDef, CompProperties_SpawnerKwamaNest props, bool ignoreRoofedRequirement, bool allowUnreachable)
		{
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 3; i++)
			{
				float minDist = props.HiveSpawnPreferredMinDist;
				bool flag;
				if (i < 2)
				{
					if (i == 1)
					{
						minDist = 0f;
					}
					flag = CellFinder.TryFindRandomReachableCellNear(pos, map, props.HiveSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), null, out intVec, 999999);
				}
				else
				{
					flag = (allowUnreachable && CellFinder.TryFindRandomCellNear(pos, map, (int)props.HiveSpawnRadius, (IntVec3 c) => CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), out intVec, -1));
				}
				if (flag)
				{
					intVec = CellFinder.FindNoWipeSpawnLocNear(intVec, map, parentDef, Rot4.North, 2, (IntVec3 c) => CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement));
					break;
				}
			}
			return intVec;
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x001B4690 File Offset: 0x001B2890
		private static bool CanSpawnHiveAt(IntVec3 c, Map map, IntVec3 parentPos, ThingDef parentDef, float minDist, bool ignoreRoofedRequirement)
		{
			if ((!ignoreRoofedRequirement && !c.Roofed(map)) || (!c.Walkable(map) || (minDist != 0f && (float)c.DistanceToSquared(parentPos) < minDist * minDist)) || c.GetFirstThing(map, ThingDefOf.MorrowRim_KwamaEggSac) != null)
			{
				return false;
			}
			if (ModSettings_Utility.MorrowRim_SettingForceKwamaNatural() && (!c.GetTerrain(map).affordances.Contains(TerrainAffordanceDefOf.SmoothableStone) && !c.GetTerrain(map).affordances.Contains(TerrainAffordanceDefOf.Diggable)))
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] is KwamaNest /*|| thingList[j] is KwamaTunnelSpawner*/)
						{
							return false;
						}
					}
				}
			}
			List<Thing> thingList2 = c.GetThingList(map);
			for (int k = 0; k < thingList2.Count; k++)
			{
				Thing thing = thingList2[k];
				if (thing.def.category == ThingCategory.Building && thing.def.passability == Traversability.Impassable && GenSpawn.SpawningWipes(parentDef, thing.def))
				{
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x001B47AA File Offset: 0x001B29AA
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Reproduce",
					icon = TexCommand.GatherSpotActive,
					action = delegate ()
					{
						KwamaNest hive;
						this.TrySpawnChildHive(false, out hive);
					}
				};
			}
			yield break;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x001B47BA File Offset: 0x001B29BA
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextHiveSpawnTick, "nextHiveSpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canSpawnHives, "canSpawnHives", true, false);
			Scribe_Values.Look<bool>(ref this.wasActivated, "wasActivated", true, false);
		}

		// Token: 0x04002D3B RID: 11579
		private int nextHiveSpawnTick = -1;

		// Token: 0x04002D3C RID: 11580
		public bool canSpawnHives = true;

		// Token: 0x04002D3D RID: 11581
		private bool wasActivated;
	}
}
