using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;

namespace MorrowRim
{
    class WeatherUtilityBlight : WeatherUtilityAsh
    {
        /* ========== pawns ========== */

        public static bool CheckAnimalBlight(Pawn p)
        {
            //need a try catch for pet checking, can cause errors when checking faction
            try
            {
                return !p.RaceProps.Humanlike && p.Faction == null && !CheckAshCloggedServos(p) 
                    && p.mindState.mentalStateHandler.CurStateDef != MentalStateDefOf.Manhunter
                    && Rand.Chance(ModSettings_Utility.SettingToFloat(ModSettings_Utility.MorrowRim_SettingBlightAnimalChance()));
            }
            catch (NullReferenceException) 
            {
                return false;
            }
        }

        public static bool CheckAnimalBlightScaling(Pawn p)
        {
            return ModSettings_Utility.MorrowRim_SettingBlightAnimalIgnoreScaling()
                || p.kindDef.combatPower <  StorytellerUtility.DefaultThreatPointsNow(p.Map) * ModSettings_Utility.MorrowRim_SettingBlightAnimalMultiplier();
        }

        public static void TriggerAnimalBlight(Pawn p)
        {
            p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, true, false, null, false);
        }

        /* ========== plants ========== */

        public static bool CheckPlantBlight(Plant p)
        {
            return !p.Blighted && p.BlightableNow && Rand.Chance(ModSettings_Utility.SettingToFloat(ModSettings_Utility.MorrowRim_SettingBlightPlantChance()));
        }

        public static void TriggerPlantBlight(Plant p)
        {
            p.CropBlighted();
        }
    }
}
