using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    [DefOf]
    public static class HediffDefOf
    {
        static HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
        }

        public static HediffDef MorrowRim_HiddenInsulted;
    }
}
