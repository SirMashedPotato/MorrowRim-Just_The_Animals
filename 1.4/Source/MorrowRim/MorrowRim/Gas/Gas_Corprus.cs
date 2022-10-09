using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using Verse.AI;
using UnityEngine;
using System.Linq;

namespace MorrowRim
{
    class Gas_Corprus : Gas
    {
        private int tickerInterval = 0;
        private int tickerMax = 120;
        static readonly String[] immuneDefs = { PawnKindDefOf.MorrowRim_CorprusLame.defName, PawnKindDefOf.MorrowRim_CorprusStalker.defName, PawnKindDefOf.MorrowRim_CorprusBloated.defName,
        "Dagoth_Slave", "Dagoth_Zombie", "Dagoth_Ghoul", "Dagoth_Vampire", "Dagoth_Ascended"};

        public override void Tick()
        {
            base.Tick();
            try{
                //need this so they don't spasm in place
                if (tickerInterval >= tickerMax)
                {
                    //make pawns vomit
                    HashSet<Thing> hashSet = new HashSet<Thing>(this.Position.GetThingList(this.Map));
                    if (hashSet != null)
                    {
                        foreach (Thing thing in hashSet)
                        {
                            //check if is pawn
                            if (thing is Pawn)
                            {
                                Pawn p = thing as Pawn;
                                if (!p.RaceProps.IsFlesh || p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_CorprusSurvived, false) != null || immuneDefs.Contains(p.def.defName))
                                {
                                    return;
                                }
                                float num = 0.028758334f;
                                if (num != 0f && !this.Destroyed)
                                {
                                    float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(p.thingIDNumber ^ 74374237));
                                    num *= num2;
                                    if (p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_Corprus) == null || (
                                        p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_Corprus) == null &&
                                        p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_Corprus).Severity + num < 1))
                                    {
                                        HealthUtility.AdjustSeverity(p, HediffDefOf.MorrowRim_Corprus, num);
                                    }
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
