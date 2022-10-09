using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using Verse.Sound;

namespace MorrowRim
{
    class CompTargetEffect_DaedraSummon : CompTargetEffect
	{
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			SummonDaedra(target);
			Find.BattleLog.Add(new BattleLogEntry_ItemUsed(user, target, this.parent.def, RulePackDefOf.Event_ItemUsed));
		}

		private static List<PawnKindDef> daedraList = new List<PawnKindDef> 
		{
			PawnKindDefOf.MorrowRim_Clannfear, PawnKindDefOf.MorrowRim_Daedroth, PawnKindDefOf.MorrowRim_Hunger, PawnKindDefOf.MorrowRim_Ogrim, PawnKindDefOf.MorrowRim_OgrimSmol, PawnKindDefOf.MorrowRim_Scamp
		};

		private static void SummonDaedra(Thing target)
		{
			PawnKindDef daedra = daedraList.RandomElement();
			Pawn newPawn = PawnGenerator.GeneratePawn(daedra, Faction.OfPlayer);
			PawnUtility.TrySpawnHatchedOrBornPawn(newPawn, target);
			PlaySound(target);
			target.Kill();
		}

		private static void PlaySound(Thing target)
        {
			SoundDef sound = SoundDefOf.Thunder_OnMap;
			sound.PlayOneShot(new TargetInfo(target.Position, target.Map, false));
		}
	}
}