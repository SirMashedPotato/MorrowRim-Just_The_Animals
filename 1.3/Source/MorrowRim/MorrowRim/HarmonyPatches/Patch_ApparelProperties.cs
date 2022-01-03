using System.Collections.Generic;
using Verse;
using RimWorld;
using HarmonyLib;

namespace MorrowRim
{

    public static class Patch_ApparelProperties
    {

        [HarmonyPatch(typeof(ThingDef), nameof(ThingDef.SpecialDisplayStats))]
        public static class CheckApparelProtective
        {

            public static void Postfix(ThingDef __instance, ref IEnumerable<StatDrawEntry> __result)
            {
                if (__instance.IsApparel)
                {
                    // ash resilience
                    AshResistant ashResistant = AshResistant.Susceptible;
                    var extendedRaceProps = ExtendedRaceProperties.Get(__instance);
                    if (extendedRaceProps != null)
                    {
                        ashResistant = extendedRaceProps.ashResistant;
                    }
                    else
                    {
                        if (__instance.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead))
                        {
                            ashResistant = AshResistant.Resistant;
                        }
                    }
                    __result = __result.AddItem(new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "MorrowRim_AshProtection".Translate(), $"MorrowRim_AshResistant_{ashResistant}".Translate(),
                        "MorrowRim_AshResistant_DescriptionApparel".Translate(), 2619));
                }
            }

        }
    }
}