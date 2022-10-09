using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;
using RimWorld;

namespace MorrowRim.Kwama
{
    [StaticConstructorOnStartup]
    public class KwamaTunnelSpawner : ThingWithComps
    {
		// Token: 0x06004C9D RID: 19613 RVA: 0x0019AD60 File Offset: 0x00198F60
		public static void ResetStaticData()
		{
			filthTypes.Clear();
			filthTypes.Add(RimWorld.ThingDefOf.Filth_Dirt);
			filthTypes.Add(RimWorld.ThingDefOf.Filth_Dirt);
			filthTypes.Add(RimWorld.ThingDefOf.Filth_Dirt);
			filthTypes.Add(RimWorld.ThingDefOf.Filth_RubbleRock);
		}
		
		// Token: 0x06004C9E RID: 19614 RVA: 0x0019ADB4 File Offset: 0x00198FB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.secondarySpawnTick, "secondarySpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnHive, "spawnHive", true, false);
			Scribe_Values.Look<float>(ref this.insectsPoints, "insectsPoints", 0f, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x0019AE14 File Offset: 0x00199014
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.secondarySpawnTick = Find.TickManager.TicksGame + this.ResultSpawnDelay.RandomInRange.SecondsToTicks();
			}
			this.CreateSustainer();
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x0019AE58 File Offset: 0x00199058
		public override void Tick()
		{
			if (base.Spawned)
			{
				this.sustainer.Maintain();
				Vector3 vector = base.Position.ToVector3Shifted();
				IntVec3 c;
				ResetStaticData();
				if (Rand.MTBEventOccurs(FilthSpawnMTB, 1f, 1.TicksToSeconds()) && CellFinder.TryFindRandomReachableCellNear(base.Position, base.Map, FilthSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c, 999999))
				{
					FilthMaker.TryMakeFilth(c, base.Map, filthTypes.RandomElement(), 1, FilthSourceFlags.None);
				}
				if (Rand.MTBEventOccurs(DustMoteSpawnMTB, 1f, 1.TicksToSeconds()))
				{
					FleckMaker.ThrowDustPuffThick(new Vector3(vector.x, 0f, vector.z)
					{
						y = AltitudeLayer.MoteOverhead.AltitudeFor()
					}, base.Map, Rand.Range(1.5f, 3f), new Color(1f, 1f, 1f, 2.5f));
				}
				if (this.secondarySpawnTick <= Find.TickManager.TicksGame)
				{
					this.sustainer.End();
					Map map = base.Map;
					IntVec3 position = base.Position;
					this.Destroy(DestroyMode.Vanish);
					if (this.spawnHive)
					{
						KwamaNest hive = (KwamaNest)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MorrowRim_KwamaNest, null), position, map, WipeMode.Vanish);
						hive.SetFaction(FactionUtility.DefaultFactionFrom(FactionDefOf.MorrowRim_Kwama), null);
						hive.questTags = this.questTags;
						foreach (CompSpawner compSpawner in hive.GetComps<CompSpawner>())
						{
							if (compSpawner.PropsSpawner.thingToSpawn == ThingDefOf.MorrowRim_KwamaEggSac)
							{
								compSpawner.TryDoSpawn();
								break;
							}
						}
					}
					/*if (this.insectsPoints > 0f)
					{
						this.insectsPoints = Mathf.Max(this.insectsPoints, Hive.spawnablePawnKinds.Min((PawnKindDef x) => x.combatPower));
						float pointsLeft = this.insectsPoints;
						List<Pawn> list = new List<Pawn>();
						int num = 0;
						Func<PawnKindDef, bool> <> 9__1;
						while (pointsLeft > 0f)
						{
							num++;
							if (num > 1000)
							{
								Log.Error("Too many iterations.", false);
								break;
							}
							IEnumerable<PawnKindDef> spawnablePawnKinds = KwamaNest.spawnablePawnKinds;
							Func<PawnKindDef, bool> predicate;
							if ((predicate = <> 9__1) == null)
							{
								predicate = (<> 9__1 = ((PawnKindDef x) => x.combatPower <= pointsLeft));
							}
							PawnKindDef pawnKindDef;
							if (!spawnablePawnKinds.Where(predicate).TryRandomElement(out pawnKindDef))
							{
								break;
							}
							Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, FactionUtility.DefaultFactionFrom(FactionDefOf.MorrowRim_Kwama));
							GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(position, map, 2, null), map, WipeMode.Vanish);
							pawn.mindState.spawnedByInfestationThingComp = this.spawnedByInfestationThingComp;
							list.Add(pawn);
							pointsLeft -= pawnKindDef.combatPower;
						}
						if (list.Any<Pawn>())
						{
							LordMaker.MakeNewLord(FactionUtility.DefaultFactionFrom(FactionDefOf.MorrowRim_Kwama), new LordJob_AssaultColony(FactionUtility.DefaultFactionFrom(FactionDefOf.MorrowRim_Kwama), true, false, false, false, true), map, list);
						}
					}*/
				}
			}
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x0019B170 File Offset: 0x00199370
		public override void Draw()
		{
			Rand.PushState();
			Rand.Seed = this.thingIDNumber;
			for (int i = 0; i < 6; i++)
			{
				this.DrawDustPart(Rand.Range(0f, 360f), Rand.Range(0.9f, 1.1f) * (float)Rand.Sign * 4f, Rand.Range(1f, 1.5f));
			}
			Rand.PopState();
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x0019B1E0 File Offset: 0x001993E0
		private void DrawDustPart(float initialAngle, float speedMultiplier, float scale)
		{
			float num = (Find.TickManager.TicksGame - this.secondarySpawnTick).TicksToSeconds();
			Vector3 pos = base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Filth);
			pos.y += 0.0454545468f * Rand.Range(0f, 1f);
			Color value = new Color(0.470588237f, 0.384313732f, 0.3254902f, 0.7f);
			matPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0f, initialAngle + speedMultiplier * num, 0f), Vector3.one * scale);
			Graphics.DrawMesh(MeshPool.plane10, matrix, TunnelMaterial, 0, null, 0, matPropertyBlock);
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x0019B29E File Offset: 0x0019949E
		private void CreateSustainer()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				SoundDef tunnel = SoundDefOf.Tunnel;
				this.sustainer = tunnel.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			});
		}

		// Token: 0x04002B0D RID: 11021
		private int secondarySpawnTick;

		// Token: 0x04002B0E RID: 11022
		public bool spawnHive = true;

		// Token: 0x04002B0F RID: 11023
		public float insectsPoints;

		// Token: 0x04002B10 RID: 11024
		public bool spawnedByInfestationThingComp;

		// Token: 0x04002B11 RID: 11025
		private Sustainer sustainer;

		// Token: 0x04002B12 RID: 11026
		private static MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04002B13 RID: 11027
		private readonly FloatRange ResultSpawnDelay = new FloatRange(26f, 30f);

		// Token: 0x04002B14 RID: 11028
		[TweakValue("Gameplay", 0f, 1f)]
		private static float DustMoteSpawnMTB = 0.2f;

		// Token: 0x04002B15 RID: 11029
		[TweakValue("Gameplay", 0f, 1f)]
		private static float FilthSpawnMTB = 0.3f;

		// Token: 0x04002B16 RID: 11030
		[TweakValue("Gameplay", 0f, 10f)]
		private static float FilthSpawnRadius = 3f;

		// Token: 0x04002B17 RID: 11031
		private static readonly Material TunnelMaterial = MaterialPool.MatFrom("Things/Filth/Grainy/GrainyA", ShaderDatabase.Transparent);

		// Token: 0x04002B18 RID: 11032
		private static List<ThingDef> filthTypes = new List<ThingDef>();
	}
}