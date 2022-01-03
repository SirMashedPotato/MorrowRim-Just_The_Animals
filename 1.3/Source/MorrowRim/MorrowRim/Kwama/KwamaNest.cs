using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class KwamaNest : ThingWithComps
    {
		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06004CF0 RID: 19696 RVA: 0x0019C896 File Offset: 0x0019AA96
		public CompCanBeDormant CompDormant
		{
			get
			{
				return base.GetComp<CompCanBeDormant>();
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06004CF1 RID: 19697 RVA: 0x0006320E File Offset: 0x0006140E
		/*Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}*/

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06004CF2 RID: 19698 RVA: 0x0019C89E File Offset: 0x0019AA9E
		public float TargetPriorityFactor
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06004CF3 RID: 19699 RVA: 0x0019C8A5 File Offset: 0x0019AAA5
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06004CF4 RID: 19700 RVA: 0x0019C8AC File Offset: 0x0019AAAC
		public CompSpawnerPawn PawnSpawner
		{
			get
			{
				return base.GetComp<CompSpawnerPawn>();
			}
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x0019C8B4 File Offset: 0x0019AAB4
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			CompCanBeDormant comp = base.GetComp<CompCanBeDormant>();
			return comp != null && !comp.Awake;
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x0019C8E0 File Offset: 0x0019AAE0
		public static void ResetStaticData()
		{
			KwamaNest.spawnablePawnKinds.Clear();
			KwamaNest.spawnablePawnKinds.Add(PawnKindDefOf.MorrowRim_KwamaScrib);
			KwamaNest.spawnablePawnKinds.Add(PawnKindDefOf.MorrowRim_KwamaForager);
			KwamaNest.spawnablePawnKinds.Add(PawnKindDefOf.MorrowRim_KwamaWorker);
			KwamaNest.spawnablePawnKinds.Add(PawnKindDefOf.MorrowRim_KwamaWarrior);
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x0019C919 File Offset: 0x0019AB19
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(FactionUtility.DefaultFactionFrom(FactionDefOf.MorrowRim_Kwama), null);
			}
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x0019C937 File Offset: 0x0019AB37
		public override void Tick()
		{
			base.Tick();
			if (base.Spawned && !this.CompDormant.Awake && !base.Position.Fogged(base.Map))
			{
				this.CompDormant.WakeUp();
			}
			if (!workerSpawned)
			{
				SpawnWorker();
			}
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x0019C974 File Offset: 0x0019AB74
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				lords[i].ReceiveMemo(KwamaNest.MemoDeSpawned);
			}
			KwamaNestUtility.Notify_HiveDespawned(this, map);
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x0019C9C4 File Offset: 0x0019ABC4
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!this.questTags.NullOrEmpty<string>())
			{
				bool flag = false;
				List<Thing> list = base.Map.listerThings.ThingsOfDef(this.def);
				for (int i = 0; i < list.Count; i++)
				{
					KwamaNest hive;
					if ((hive = (list[i] as KwamaNest)) != null && hive != this && hive.CompDormant.Awake && !hive.questTags.NullOrEmpty<string>() && QuestUtility.AnyMatchingTags(hive.questTags, this.questTags))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "AllHivesDestroyed");
				}
			}
			base.Destroy(mode);
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x0019CA6C File Offset: 0x0019AC6C
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def.ExternalViolenceFor(this) && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				Lord lord = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord != null)
				{
					lord.ReceiveMemo(KwamaNest.MemoAttackedByEnemy);
				}
			}
			if (dinfo.Def == DamageDefOf.Flame && (float)this.HitPoints < (float)base.MaxHitPoints * 0.3f)
			{
				Lord lord2 = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord2 != null)
				{
					lord2.ReceiveMemo(KwamaNest.MemoBurnedBadly);
				}
			}
			base.PostApplyDamage(dinfo, totalDamageDealt);
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x0019CB00 File Offset: 0x0019AD00
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.Spawned && (dinfo == null || dinfo.Value.Category != DamageInfo.SourceCategory.Collapse))
			{
				List<Lord> lords = base.Map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					lords[i].ReceiveMemo(KwamaNest.MemoDestroyedNonRoofCollapse);
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x0019CB70 File Offset: 0x0019AD70
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (this.PawnSpawner.spawnedPawns.Count > 0)
			{
				if (this.PawnSpawner.spawnedPawns.Any((Pawn p) => !p.Downed))
				{
					reason = this.def.label;
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x0019CBD4 File Offset: 0x0019ADD4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x0019CBE4 File Offset: 0x0019ADE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.workerSpawned, "spawnWorkerOnce", true, false);
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				bool flag = false;
				Scribe_Values.Look<bool>(ref flag, "active", false, false);
				if (flag)
				{
					this.CompDormant.WakeUp();
				}
			}
		}

		public void SpawnWorker()
		{
			Lord lord = base.GetComp<CompSpawnerPawn>().Lord;
			if (lord != null && this.PawnSpawner.spawnedPawns.Count > 0 && this.PawnSpawner.spawnedPawns.FindAll(x => x.def.defName == PawnKindDefOf.MorrowRim_KwamaWorker.defName).Count == 0)
			{
				Pawn pawn;
				pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.MorrowRim_KwamaWorker, this.Faction, PawnGenerationContext.NonPlayer));
				this.PawnSpawner.spawnedPawns.Add(pawn);
				GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(this.Position, this.Map, PawnSpawnRadius, null), this.Map, WipeMode.Vanish);
				lord.AddPawn(pawn);
			}
			if (lord != null && this.PawnSpawner.spawnedPawns.Count > 0)
			{
				workerSpawned = true;
			}
		}

		// Token: 0x04002B43 RID: 11075
		public const int PawnSpawnRadius = 2;

		// Token: 0x04002B44 RID: 11076
		public const float MaxSpawnedPawnsPoints = 500f;

		// Token: 0x04002B45 RID: 11077
		public const float InitialPawnsPoints = 200f;

		// Token: 0x04002B46 RID: 11078
		public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

		// Token: 0x04002B47 RID: 11079
		public static readonly string MemoAttackedByEnemy = "KwamaNestAttacked";

		// Token: 0x04002B48 RID: 11080
		public static readonly string MemoDeSpawned = "KwamaNestDeSpawned";

		// Token: 0x04002B49 RID: 11081
		public static readonly string MemoBurnedBadly = "KwamaNestBurnedBadly";

		// Token: 0x04002B4A RID: 11082
		public static readonly string MemoDestroyedNonRoofCollapse = "KwamaNestDestroyedNonRoofCollapse";

		private bool workerSpawned = false;
	}
}
