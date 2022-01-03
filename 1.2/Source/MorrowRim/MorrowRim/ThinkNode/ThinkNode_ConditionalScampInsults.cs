using Verse;
using System;
using RimWorld;
using Verse.AI;

namespace MorrowRim
{
    class ThinkNode_ConditionalScampInsults : ThinkNode_Conditional
    {

        protected override bool Satisfied(Pawn pawn)
        {
            return ModSettings_Utility.MorrowRim_SettingEnableScampInsults();
        }
    }
}
