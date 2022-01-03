using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;

namespace MorrowRim
{
    class WeatherUtilityAsh
    {

        /* ========== generic checks ========== */

        public static bool IsValidTarget(Thing t)
        {
            return t.Spawned && !t.Position.Roofed(t.Map) && !t.Position.Fogged(t.Map);
        }

        public static bool IsValidCell(IntVec3 x, Map m)
        {
            return !x.Roofed(m) && !x.Fogged(m);
        }

        public static bool IsImmuneToAsh(Thing t)
        {
            var extendedRaceProps = ExtendedRaceProperties.Get(t.def);
            return extendedRaceProps != null && extendedRaceProps.ashResistant == AshResistant.Resistant;
        }

        public static bool IsMechanicalToAsh(Thing t)
        {
            var extendedRaceProps = ExtendedRaceProperties.Get(t.def);
            return extendedRaceProps != null && extendedRaceProps.ashResistant == AshResistant.Mechanical;
        }

        /* ========== pawns ========== */

        public static bool ExtendedImmuneToAshCheck(Pawn p)
        {
            return p.RaceProps.FleshType == FleshTypeDefOf.Insectoid || p.RaceProps.FleshType.defName == "ROM_StrangeFlesh" ||
                !p.def.race.body.HasPartWithTag(BodyPartTagDefOf.BreathingPathway) || !p.def.race.body.HasPartWithTag(BodyPartTagDefOf.BreathingSource);
        }

        //check for zombieland zombies
        public static bool ZombielandCheck(Pawn p)
        {
            return ModSettings_Utility.CheckZombieland() && ModSettings_Utility.MorrowRim_SettingZombielandIntegration() && p.def.defName == "Zombie";
        }

        /* servos */

        public static bool CheckAshCloggedServos(Pawn p)
        {
            return IsMechanicalToAsh(p) || p.RaceProps.FleshType == FleshTypeDefOf.Mechanoid
                /* mod specific checks */
                || p.RaceProps.FleshType.defName == "Android" || p.RaceProps.FleshType.defName == "ChJDroid" || p.def.defName == "ChjAndroid";
        }

        public static void TriggerAshCloggedServos(Pawn p)
        {
            if (Rand.Chance(ModSettings_Utility.SettingToFloat(ModSettings_Utility.MorrowRim_SettingAshCloggedServo())))
            {
                BodyPartRecord part = p.RaceProps.body.AllParts.RandomElement();
                p.health.AddHediff(HediffDefOf.MorrowRim_AshCloggedServo, part);
            }
            return;
        }

        /* apparel */

        //for mod conmpatability, should swap to patching in modExtension
        private static readonly string[] apparelList = new string[] {
            /*Rimatomics*/ 
            "Apparel_RadiationMask", "Apparel_MoppMaskDesert", "Apparel_MoppMaskWoodland", "Apparel_MoppMaskSnow"
        };

        //for checking if ash buildup is triggered, doesn't need to specifically check eyes
        public static bool CheckApparel(Pawn p)
        {
            return p.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.FullHead) || p.apparel.WornApparel.Any(x => IsImmuneToAsh(x))
                || p.apparel.WornApparel.Any(x => apparelList.Contains(x.def.defName.ToString()));
        }

        /* eyes */

        //checks to see if target is likely to be valid
        public static bool CheckAshFilledEyes(Pawn p)
        {
            return (!p.def.race.Humanlike && p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource) != null) 
                || (p.def.race.Humanlike && p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource) != null 
                && !p.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.Eyes) && !p.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.UpperHead) && !p.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.FullHead));
        }

        //ensures thing is actually an eye
        public static BodyPartRecord GetAshFilledEyes(Pawn p)
        {
            List<BodyPartRecord> parts =  p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource).Where(x => x.Label.Contains("eye") || x.Label.Contains("Eye")).ToList();
            if (!parts.NullOrEmpty())
            {
                return parts.RandomElement();
            }
            return null;
        }

        public static void TriggerAshFilledEyes(Pawn p, BodyPartRecord part)
        {
            if (Rand.Chance(ModSettings_Utility.SettingToFloat(ModSettings_Utility.MorrowRim_SettingAshFilledEye())))
            {
                if (part != null && !p.health.hediffSet.PartIsMissing(part))
                {
                    p.health.AddHediff(HediffDefOf.MorrowRim_AshInEyes, part);
                }
            }
        }

        /* misc pawn */

        public static bool CanBreathe(Pawn p)
        {
            return p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.BreathingSource) != null;
        }

        public static bool HasImplant(Pawn p)
        {
            return p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_BionicFilter) != null;
        }

        /* New version of HasImplant
         * checks for any hediff with the extendedRaceProps extension
         */

        public static bool HasImmunityHediff(Pawn p)
        {
            foreach(Hediff h in p.health.hediffSet.hediffs)
            {
                var extendedRaceProps = ExtendedRaceProperties.Get(h.def);
                if (extendedRaceProps != null && extendedRaceProps.ashResistant == AshResistant.Resistant)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasImmunityTrait(Pawn p)
        {
            foreach (Trait t in p.story.traits.allTraits)
            {
                var extendedRaceProps = ExtendedRaceProperties.Get(t.def);
                if (extendedRaceProps != null && extendedRaceProps.ashResistant == AshResistant.Resistant)
                {
                    return true;
                }
            }
            return false;
        }

        public static void IncreaseAshBuildup(Pawn p)
        {
            float num = 0.01f * ModSettings_Utility.MorrowRim_SettingAshBuildupMultiplier();
            num /= (p.BodySize / p.health.capacities.GetLevel(PawnCapacityDefOf.Breathing));
            if (num != 0f)
            {
                float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(p.thingIDNumber ^ 74374237));
                num *= num2;
                HealthUtility.AdjustSeverity(p, HediffDefOf.MorrowRim_AshBuildup, num);
            }
        }

        /* ========== plants ========== */

        public static bool IsWindPlant(Thing thing)
        {
            return thing is Building && Rand.RangeInclusive(1, 10) > 7 
                && (thing.def.defName.Equals("WindTurbine") || thing.def.defName.Equals("Windmill"));
        }

        public static void DamageWindPlant(Thing thing)
        {
            DamageInfo info = new DamageInfo
            {
                Def = DamageDefOf.Deterioration,
            };
            //takes damage 14 times becuase of building size
            info.SetAmount(Rand.Gaussian(ModSettings_Utility.MorrowRim_SettingAshTurbineDamage()));
            thing.TakeDamage(info);
        }

        public static bool IsActualPlant(Thing thing)
        {
            return thing is Plant;
        }

        public static bool IsValidPlant(Plant plant)
        {
            return !IsImmuneToAsh(plant) && Rand.Chance(ModSettings_Utility.SettingToFloat(ModSettings_Utility.MorrowRim_SettingAshPlantChance()));
        }

        public static bool IsSownPlant(Plant plant)
        {
            return plant.sown;
        }

        public static void DamagePlant(Plant plant)
        {
            DamageInfo info = new DamageInfo
            {
                Def = DamageDefOf.Deterioration
            };
            info.SetAmount(Rand.Gaussian(ModSettings_Utility.MorrowRim_SettingAshPlantDamage()));
            if (plant.def == ThingDefOf.MorrowRim_MuckSponge && !ModSettings_Utility.MorrowRim_SettingAshIgnoreResistance())
            {
                info.SetAmount(info.Amount / 2);
            }
            plant.TakeDamage(info);
        }
    }
}