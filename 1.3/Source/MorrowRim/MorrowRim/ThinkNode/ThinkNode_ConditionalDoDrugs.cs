using Verse;
using RimWorld;

namespace MorrowRim
{
    class ThinkNode_ConditionalDoDrugs : ThinkNode_ConditionalPawnKind
    {

        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.kindDef == this.pawnKind && ModSettings_Utility.MorrowRim_SettingEnableScampBehaviour();
        }
    }
}
