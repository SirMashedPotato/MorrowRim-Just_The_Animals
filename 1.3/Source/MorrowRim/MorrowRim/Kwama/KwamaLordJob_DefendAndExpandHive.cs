using System;
using Verse;
using Verse.AI.Group;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class KwamaLordJob_DefendAndExpandHive : LordJob
    {
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x00010226 File Offset: 0x0000E426
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x00010226 File Offset: 0x0000E426
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x000F15DE File Offset: 0x000EF7DE
		public KwamaLordJob_DefendAndExpandHive()
		{
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x00110908 File Offset: 0x0010EB08
		public KwamaLordJob_DefendAndExpandHive(SpawnedPawnParams parms)
		{
			this.aggressive = parms.aggressive;
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x0011091C File Offset: 0x0010EB1C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			KwamaLordToil_DefendAndExpandHive lordToil_DefendAndExpandHive = new KwamaLordToil_DefendAndExpandHive();
			lordToil_DefendAndExpandHive.distToHiveToAttack = 0f;
			stateGraph.StartingToil = lordToil_DefendAndExpandHive;
			KwamaLordToil_DefendHiveAggressively lordToil_DefendHiveAggressively = new KwamaLordToil_DefendHiveAggressively();
			lordToil_DefendHiveAggressively.distToHiveToAttack = 40f;
			stateGraph.AddToil(lordToil_DefendHiveAggressively);
			KwamaLordToil_AssaultColony lordToil_AssaultColony = new KwamaLordToil_AssaultColony(false);
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendAndExpandHive, lordToil_DefendHiveAggressively, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null));
			transition.AddTrigger(new Trigger_PawnLostViolently(false));
			transition.AddTrigger(new Trigger_Memo(KwamaNest.MemoAttackedByEnemy));
			transition.AddTrigger(new Trigger_Memo(KwamaNest.MemoBurnedBadly));
			transition.AddTrigger(new Trigger_Memo(KwamaNest.MemoDestroyedNonRoofCollapse));
			transition.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendAndExpandHive, lordToil_AssaultColony, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendHiveAggressively, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_DefendAndExpandHive, lordToil_DefendAndExpandHive, true, true);
			transition4.AddTrigger(new Trigger_Memo(KwamaNest.MemoDeSpawned));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_DefendHiveAggressively, lordToil_DefendHiveAggressively, true, true);
			transition5.AddTrigger(new Trigger_Memo(KwamaNest.MemoDeSpawned));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_AssaultColony, lordToil_DefendAndExpandHive, false, true);
			transition6.AddSource(lordToil_DefendHiveAggressively);
			transition6.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(900, new string[]
			{
				KwamaNest.MemoAttackedByEnemy,
				KwamaNest.MemoBurnedBadly,
				KwamaNest.MemoDestroyedNonRoofCollapse,
				KwamaNest.MemoDeSpawned,
				HediffGiver_Heat.MemoPawnBurnedByAir
			}));
			transition6.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x00110B28 File Offset: 0x0010ED28
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
		}

		// Token: 0x04001AD3 RID: 6867
		private bool aggressive;
	}
}
