using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;

namespace MorrowRim
{
    public class WeatherEvent_FireDamage : WeatherEvent
    {
        public WeatherEvent_FireDamage(Map map) : base(map)
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
            if (!ModSettings_Utility.MorrowRim_SettingEnableFireEffects()) return;
            var props = WeatherProperties.Get(map.weatherManager.curWeather);
            if (props != null)
            {
                if (props.setFireToPlants)
                {
                    //for plants, first to avoid out of bounds exception
                    IntVec3[] vecs = new IntVec3[map.AllCells.Where(x => WeatherUtilityAsh.IsValidCell(x, this.map)).Count()];
                    vecs = map.AllCells.Where(x => WeatherUtilityAsh.IsValidCell(x, this.map)).InRandomOrder().ToArray();
                    foreach (IntVec3 cell in vecs)
                    {
                        DoFireDamageToPlant(cell);
                    }
                }

                //for pawns, in a try catch incase a pawn dies during, which can cause an out of bounds exception
                try
                {
                    List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
                    for (int i = 0; i != allPawnsSpawned.Count(); i++)
                    {
                        if (WeatherUtilityAsh.IsValidTarget(allPawnsSpawned[i]))
                        {
                            DoFireDamageToPawn(allPawnsSpawned[i]);
                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Log.Message("Fire damage fire event had argument out of bounds exception occur, may have ended prematurely. This is completely safe to ignore.");
                }
            }
        }

        public void DoFireDamageToPawn(Pawn p)
        {
            //Is mechanical, return
            if (WeatherUtilityAsh.CheckAshCloggedServos(p))
            {
                return;
            }

            var props = WeatherProperties.Get(map.weatherManager.curWeather);
            if (props != null)
            {
                //check for success, if so burn them
                if (props.setFireToPawns)
                {
                    if (WeatherUtilityFire.GetChanceOfFirePawn(p))
                    {
                        WeatherUtilityFire.IgnitePawn(p, props.fireSize);
                    }
                } 
                else
                {
                    if (WeatherUtilityFire.GetChanceOfBurn(p) && WeatherUtilityFire.GetBodyPart(p, out BodyPartRecord bpr))
                    {
                        WeatherUtilityFire.BurnPawn(p, props.burnDamage, bpr);
                    }
                }
            }
        }

        public void DoFireDamageToPlant(IntVec3 vec)
        {
            List<Thing> thingList = vec.GetThingList(this.map);
            foreach (Thing t in thingList)
            {
                if (WeatherUtilityAsh.IsActualPlant(t))
                {
                    Plant p = t as Plant;
                    var props = WeatherProperties.Get(map.weatherManager.curWeather);
                    if (props != null)
                    {
                        if (WeatherUtilityAsh.IsValidPlant(p) && WeatherUtilityFire.GetChanceOfFirePlant(p))
                        {
                            if (ModSettings_Utility.MorrowRim_SettingFireOnlySownPlants() && !WeatherUtilityAsh.IsSownPlant(p))
                            {
                                return;
                            }
                            WeatherUtilityFire.IgnitePlant(p, props.fireSize);
                        }
                    }
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
