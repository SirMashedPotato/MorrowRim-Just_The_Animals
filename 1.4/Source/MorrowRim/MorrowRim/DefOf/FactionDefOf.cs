using RimWorld;

namespace MorrowRim
{
    [DefOf]
    public static class FactionDefOf
    {
        static FactionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
        }
        public static FactionDef MorrowRim_Kwama;

    }
}