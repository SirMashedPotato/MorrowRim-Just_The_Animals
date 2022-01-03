using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    [DefOf]
    public static class ResearchProjectDefOf
    {
        static ResearchProjectDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectDefOf));
        }

        public static ResearchProjectDef MorrowRim_AshSowing;

    }
}
