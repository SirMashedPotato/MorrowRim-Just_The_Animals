using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    [DefOf]
    public static class ThingDefOf
    {
        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }

        public static ThingDef MorrowRim_SiltStrider;
        public static ThingDef MorrowRim_CliffRacer;
        public static ThingDef MorrowRim_RetchingNetch;
        public static ThingDef MorrowRim_AlbinoGuar;
        public static ThingDef MorrowRim_Shalk;
        public static ThingDef MorrowRim_AshHopper;
        public static ThingDef MorrowRim_NixOx;
        public static ThingDef MorrowRim_FungalShalk;

        public static ThingDef Gas_RetchingNetch;

        public static ThingDef MorrowRim_KwamaScrib;
        public static ThingDef MorrowRim_KwamaForager;
        public static ThingDef MorrowRim_KwamaWorker;
        public static ThingDef MorrowRim_KwamaWarrior;

    }
}
