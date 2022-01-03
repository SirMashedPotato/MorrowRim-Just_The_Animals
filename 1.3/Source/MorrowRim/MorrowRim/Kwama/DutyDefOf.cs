using System;
using Verse.AI;
using RimWorld;

namespace MorrowRim.Kwama
{
    [DefOf]
    public static class DutyDefOf
    {
        static DutyDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DutyDefOf));
        }

        public static DutyDef KwamaDefendAndExpandHive;
        public static DutyDef KwamaDefendHiveAggressively;
        public static DutyDef KwamaAssaultColony;
    }
}
