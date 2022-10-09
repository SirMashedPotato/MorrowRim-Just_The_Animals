using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class KwamaLordToil_AssaultColony : LordToil
    {
		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x000101AD File Offset: 0x0000E3AD
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x00114DAD File Offset: 0x00112FAD
		public KwamaLordToil_AssaultColony(bool attackDownedIfStarving = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060031D2 RID: 12754 RVA: 0x00010226 File Offset: 0x0000E426
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x00114DBC File Offset: 0x00112FBC
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x00114DD0 File Offset: 0x00112FD0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.KwamaAssaultColony);
				this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
			}
			if (lord.faction.RelationKindWith(Faction.OfPlayer) != FactionRelationKind.Hostile)
			{
				Faction.OfPlayer.SetRelationDirect(lord.faction, FactionRelationKind.Hostile, false, "Nest threatened");
				Find.LetterStack.ReceiveLetter("LetterLabelKwamaNestThreatened".Translate().CapitalizeFirst(), "LetterKwamaNestThreatened".Translate(), LetterDefOf.NegativeEvent, null, null, null);
			}
		}

		// Token: 0x04001B19 RID: 6937
		private bool attackDownedIfStarving;
	}
}
