using System.Collections.Generic;
using Verse;
using RimWorld;
using HarmonyLib;

namespace MorrowRim
{

    public static class Patch_RaceProperties
    {
        [HarmonyPatch(typeof(RaceProperties), nameof(RaceProperties.SpecialDisplayStats))]
        public static class Patch_SpecialDisplayStats
        {

            public static void Postfix(ThingDef parentDef, ref IEnumerable<StatDrawEntry> __result)
            {
                // ash resilience
                AshResistant ashResistant = AshResistant.Susceptible;
                var extendedRaceProps = ExtendedRaceProperties.Get(parentDef);
                if (extendedRaceProps != null)
                {
                    //Log.Message(parentDef.defName + " already has: " + extendedRaceProps.ashResistant);
                    ashResistant = extendedRaceProps.ashResistant;
                }
                else
                {
                    if (parentDef.race.FleshType == FleshTypeDefOf.Mechanoid || parentDef.race.FleshType.defName == "Android" || parentDef.race.FleshType.defName == "ChJDroid" || parentDef.defName == "ChjAndroid")
                    {
                        ashResistant = AshResistant.Mechanical;
                    }
                    else if (!parentDef.race.body.HasPartWithTag(BodyPartTagDefOf.BreathingPathway) || !parentDef.race.body.HasPartWithTag(BodyPartTagDefOf.BreathingSource) || parentDef.race.FleshType == FleshTypeDefOf.Insectoid)
                    {
                        ashResistant = AshResistant.Resistant;
                    }
                }

                __result = __result.AddItem(new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "MorrowRim_AshResistant".Translate(), $"MorrowRim_AshResistant_{ashResistant}".Translate(),
                    "MorrowRim_AshResistant_Description".Translate(), 2619));
            }
        }
    }
}