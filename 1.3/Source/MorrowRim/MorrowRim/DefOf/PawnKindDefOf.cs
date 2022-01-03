using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    [DefOf]
    public static class PawnKindDefOf
    {
        static PawnKindDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf));
        }

        public static PawnKindDef MorrowRim_SiltStrider;
        public static PawnKindDef MorrowRim_AlbinoGuar;
        public static PawnKindDef MorrowRim_CliffRacer;

        //kwama
        public static PawnKindDef MorrowRim_KwamaScrib;
        public static PawnKindDef MorrowRim_KwamaForager;
        public static PawnKindDef MorrowRim_KwamaWorker;
        public static PawnKindDef MorrowRim_KwamaWarrior;

        //daedra
        public static PawnKindDef MorrowRim_Clannfear;
        public static PawnKindDef MorrowRim_Daedroth;
        public static PawnKindDef MorrowRim_Scamp;
        public static PawnKindDef MorrowRim_Ogrim;
        public static PawnKindDef MorrowRim_Hunger;
        public static PawnKindDef MorrowRim_OgrimSmol;
    }
}
