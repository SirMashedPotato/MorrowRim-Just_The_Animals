using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;

namespace MorrowRim
{
    class WeatherUtilityFire
    {
        /* ========== generic checks ========== */

        /* ========== pawns ========== */

        public static bool GetChanceOfBurn(Pawn p)
        {
            return Rand.Chance(ModSettings_Utility.SettingToFloat(ModSettings_Utility.MorrowRim_SettingFireFirePawnChance()) * p.RaceProps.baseBodySize);
        }

        public static bool GetChanceOfFirePawn(Pawn p)
        {
            return Rand.Chance(ModSettings_Utility.SettingToFloat(ModSettings_Utility.MorrowRim_SettingFireFirePawnChance()) * p.def.BaseFlammability);
        }

        public static void BurnPawn(Pawn p, float f, BodyPartRecord bpr)
        {
            DamageInfo dinfo = new DamageInfo(DamageDefOf.Burn, f, 0, -1, null, bpr);
            p.TakeDamage(dinfo);
        }

        public static bool GetBodyPart(Pawn p, out BodyPartRecord bpr)
        {
            foreach(BodyPartRecord part in p.RaceProps.body.AllPartsVulnerableToFrostbite.InRandomOrder())
            {
                foreach(BodyPartGroupDef x in part.groups)
                {
                    if (p.RaceProps.Animal || !p.apparel.BodyPartGroupIsCovered(x))
                    {
                        bpr = part;
                        return true;
                    }
                }
            }
            bpr = null;
            return false;
        }

        public static void IgnitePawn(Pawn p, float f)
        {
            p.TryAttachFire(f);
        }

        /* ========== plants ========== */

        public static bool GetChanceOfFirePlant(Plant p)
        {
            return Rand.Chance(ModSettings_Utility.SettingToFloat(ModSettings_Utility.MorrowRim_SettingFireFirePlantChance()) * p.def.BaseFlammability);
        }

        public static void IgnitePlant(Plant p, float f)
        {
            GenExplosion.DoExplosion(p.Position, p.Map, 1f, DamageDefOf.Flame, null, 1);
        }

        /* ========== misc ========== */
    }
}
