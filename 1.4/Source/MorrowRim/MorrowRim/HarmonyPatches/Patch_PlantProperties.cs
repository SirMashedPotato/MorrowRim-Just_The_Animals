using System.Collections.Generic;
using Verse;
using RimWorld;
using HarmonyLib;

namespace MorrowRim
{

    public static class Patch_PlantProperties
    {

        [HarmonyPatch(typeof(ThingDef), nameof(ThingDef.SpecialDisplayStats))]
        public static class CheckPlantForImmune
        {

            public static void Postfix(ThingDef __instance, ref IEnumerable<StatDrawEntry> __result)
            {
                if (__instance.category == ThingCategory.Plant)
                {
                    // ash resilience
                    AshResistant ashResistant = AshResistant.Susceptible;
                    var extendedRaceProps = ExtendedRaceProperties.Get(__instance);
                    if (extendedRaceProps != null)
                    {
                        ashResistant = extendedRaceProps.ashResistant;
                    }
                    __result = __result.AddItem(new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "MorrowRim_AshResistant".Translate(), $"MorrowRim_AshResistant_{ashResistant}".Translate(),
                        "MorrowRim_AshResistant_DescriptionPlant".Translate(), 2619));
                }
            }

        }
    }
}