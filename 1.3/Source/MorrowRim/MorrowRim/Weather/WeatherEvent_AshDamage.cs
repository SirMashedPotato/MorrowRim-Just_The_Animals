using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;

namespace MorrowRim
{
    public class WeatherEvent_AshDamage : WeatherEvent
    {
        public WeatherEvent_AshDamage(Map map) : base(map)
        {
        }

        public override bool Expired
        {
            get
            {
                return this.age > this.duration;
            }
        }

        public override void FireEvent()
        {
            if (!ModSettings_Utility.MorrowRim_SettingEnableAshEffects()) return;

            //for plants, first to avoid out of bounds exception
            IntVec3[] vecs = new IntVec3[map.AllCells.Where(x => WeatherUtilityAsh.IsValidCell(x, this.map)).Count()];
            vecs = map.AllCells.Where(x => WeatherUtilityAsh.IsValidCell(x, this.map)).InRandomOrder().ToArray();
            foreach (IntVec3 cell in vecs)
            {
                DoAshDamageToPlant(cell);
            }

            //for pawns, in a try catch incase a pawn dies during, which can cause an out of bounds exception
            try
            {
                List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
                for (int i = 0; i != allPawnsSpawned.Count(); i++)
                {
                    if (WeatherUtilityAsh.IsValidTarget(allPawnsSpawned[i]))
                    {
                        DoAshDamageToPawn(allPawnsSpawned[i]);
                    }
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                Log.Message("Ash damage fire event had argument out of bounds exception occur, may have ended prematurely. This is completely safe to ignore.");
            }
        }

        public void DoAshDamageToPawn(Pawn p)
        {
            //Is mechanical, try inflict clogged servos, then return
            if (WeatherUtilityAsh.CheckAshCloggedServos(p))
            {
                WeatherUtilityAsh.TriggerAshCloggedServos(p);
                return;
            }

            //try inflict filled eyes
            if (WeatherUtilityAsh.CheckAshFilledEyes(p))
            {
                BodyPartRecord part = WeatherUtilityAsh.GetAshFilledEyes(p);
                if (part != null) 
                {
                    WeatherUtilityAsh.TriggerAshFilledEyes(p, part);
                }
            }

            //check apparel
            if (p.RaceProps.Humanlike && WeatherUtilityAsh.CheckApparel(p))
            {
                return;
            }

            //check if buildup is blocked
            if (!WeatherUtilityAsh.CanBreathe(p) || WeatherUtilityAsh.HasImmunityHediff(p))
            {
                return;
            }

            //check for blocking trait
            if (p.RaceProps.Humanlike && WeatherUtilityAsh.HasImmunityTrait(p))
            {
                return;
            }

            //extra check for Zombieland, done in code to make it toggleable
            if (WeatherUtilityAsh.ZombielandCheck(p))
            {
                return;
            }

            //actual ash buildup
            if(ModSettings_Utility.MorrowRim_SettingAshIgnoreResistance() || (!WeatherUtilityAsh.IsImmuneToAsh(p) && !WeatherUtilityAsh.ExtendedImmuneToAshCheck(p)))
            {
                WeatherUtilityAsh.IncreaseAshBuildup(p);
            }
        }

        public void DoAshDamageToPlant(IntVec3 vec)
        {
            List<Thing> thingList = vec.GetThingList(this.map);
            foreach(Thing t in thingList)
            {
                if (WeatherUtilityAsh.IsActualPlant(t))
                {
                    Plant p = t as Plant;
                    if (ModSettings_Utility.MorrowRim_SettingAshIgnoreResistance() || WeatherUtilityAsh.IsValidPlant(p))
                    {
                        if (ModSettings_Utility.MorrowRim_SettingAshOnlySownPlants() && !WeatherUtilityAsh.IsSownPlant(p))
                        {
                            return;
                        }
                        WeatherUtilityAsh.DamagePlant(p);
                    }
                    return;
                }
                if(WeatherUtilityAsh.IsWindPlant(t) && Rand.Chance(0.5f))
                {
                    WeatherUtilityAsh.DamageWindPlant(t);
                    return;
                }
            }
        }

        public override void WeatherEventTick()
        {
            this.age++;
        }

        //variables
        private int age;
        private int duration;
    }
}
