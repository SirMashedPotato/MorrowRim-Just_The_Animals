using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace MorrowRim.Kwama
{
    public class KwamaThinkNode_ConditionalHiveCanReproduce : ThinkNode_Conditional
    {
		// Token: 0x06003337 RID: 13111 RVA: 0x0011A0CC File Offset: 0x001182CC
		protected override bool Satisfied(Pawn pawn)
		{
			KwamaNest hive = pawn.mindState.duty.focus.Thing as KwamaNest;
			return hive != null && hive.GetComp<CompSpawnerKwamaNest>().canSpawnHives;
		}
	}
}
