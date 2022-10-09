using System;
using Verse.AI;
using RimWorld;
using Verse;

namespace MorrowRim.Kwama
{
    class KwamaLordToil_DefendHiveAggressively : KwamaLordToil_HiveRelated
    {
		// Token: 0x060031E9 RID: 12777 RVA: 0x0011535C File Offset: 0x0011355C
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				KwamaNest hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.KwamaDefendHiveAggressively, hiveFor, this.distToHiveToAttack);
				this.lord.ownedPawns[i].mindState.duty = duty;
				//Log.Message("kwama unit " + i + ", " + this.lord.ownedPawns[i].def.defName + ", switching to defend hive agressively");
			}
			if (lord.faction.RelationKindWith(Faction.OfPlayer) != FactionRelationKind.Hostile)
			{
				Faction.OfPlayer.SetRelationDirect(lord.faction, FactionRelationKind.Hostile, false, "Nest threatened");
				Find.LetterStack.ReceiveLetter("LetterLabelKwamaNestThreatened".Translate().CapitalizeFirst(), "LetterKwamaNestThreatened".Translate(), LetterDefOf.NegativeEvent, null, null, null);
			}
		}

		// Token: 0x04001B1F RID: 6943
		public float distToHiveToAttack = 30f;
	}
}
