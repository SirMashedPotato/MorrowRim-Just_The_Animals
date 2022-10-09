using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class KwamaThinkNode_ConditionalPawnKindDef : ThinkNode_Conditional
    {
		// Token: 0x060032D8 RID: 13016 RVA: 0x00119978 File Offset: 0x00117B78
		protected override bool Satisfied(Pawn pawn)
		{
			pawnKind = PawnKindDefOf.MorrowRim_KwamaWorker.defName;
			//if (pawn.kindDef.defName == pawnKind) Log.Message("match between: " + pawn.kindDef.defName + ", " + pawnKind);
			//else Log.Message("no match between: " + pawn.kindDef.defName + ", " + pawnKind);
			return pawnKind == pawn.kindDef.defName;
		}

		public string pawnKind;
	}
}
