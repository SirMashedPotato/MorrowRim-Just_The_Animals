using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;


namespace MorrowRim
{
    class Gas_AshStave : Gas
    {
        private int tickerInterval = 0;
        private int tickerMax = 120;
        public override void Tick()
        {
            base.Tick();
            try
            {
                if (tickerInterval >= tickerMax)
                {
                    HashSet<Thing> hashSet = new HashSet<Thing>(this.Position.GetThingList(this.Map));
                    if (hashSet != null)
                    {
                        foreach (Thing thing in hashSet)
                        {
                            //check if is pawn
                            if (thing != null && thing is Pawn)
                            {
                                Pawn p = thing as Pawn;
                                var extendedRaceProps = ExtendedRaceProperties.Get(p.def);
                                //check if ash resistant
                                if (extendedRaceProps != null && extendedRaceProps.ashResistant == AshResistant.Resistant)
                                {
                                    return;
                                }

                                //check if they can actually breathe
                                if (p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.BreathingSource) == null)
                                {
                                    return;
                                }

                                //check for filter implant
                                if (p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_BionicFilter) != null)
                                {
                                    return;
                                }

                                //increase ash buildup
                                float num = 0.01f;
                                num /= (p.BodySize / p.health.capacities.GetLevel(PawnCapacityDefOf.Breathing));
                                if (num != 0f)
                                {
                                    float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(p.thingIDNumber ^ 74374237));
                                    num *= num2;
                                    HealthUtility.AdjustSeverity(p, HediffDefOf.MorrowRim_AshBuildup, num * 2);
                                }
                            }
                        }
                    }
                    tickerInterval = 0;
                }
                tickerInterval++;
            }
            catch (NullReferenceException e)
            {

            }
        }
    }
}

