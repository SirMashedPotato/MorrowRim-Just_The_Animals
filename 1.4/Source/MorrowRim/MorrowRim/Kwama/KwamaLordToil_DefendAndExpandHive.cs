using System;
using Verse.AI;
using RimWorld;
using Verse;

namespace MorrowRim.Kwama
{
    class KwamaLordToil_DefendAndExpandHive : KwamaLordToil_HiveRelated
    {
		// Token: 0x060031E4 RID: 12772 RVA: 0x0011525C File Offset: 0x0011345C
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				KwamaNest hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.KwamaDefendAndExpandHive, hiveFor, this.distToHiveToAttack);
				this.lord.ownedPawns[i].mindState.duty = duty;
				//Log.Message("kwama unit " + i + ", " + this.lord.ownedPawns[i].def.defName + ", switching to defend and expand hive");
			}
			if (lord.faction.RelationKindWith(Faction.OfPlayer) == FactionRelationKind.Hostile)
			{
				Faction.OfPlayer.SetRelationDirect(lord.faction, FactionRelationKind.Neutral, false, "Nest calmed");
				Find.LetterStack.ReceiveLetter("LetterLabelKwamaNestCalmed".Translate().CapitalizeFirst(), "LetterKwamaNestCalmed".Translate(), LetterDefOf.PositiveEvent, null, null, null);
			}
		}

		// Token: 0x04001B1D RID: 6941
		public float distToHiveToAttack = 10f;
	}
}
