using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using Verse.AI;


namespace MorrowRim
{
    public class Gas_RetchingNetch : Gas
    {
        private int tickerInterval = 0;
        private int tickerMax = 120;

        public override void Tick()
        {
            base.Tick();
            try
            {
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

                                //check if not mechanoid, can breathe and is not retching netch
                                if (!p.RaceProps.IsMechanoid && p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.BreathingSource) != null && p.def.defName != String.Concat(ThingDefOf.MorrowRim_RetchingNetch))
                                {
                                    p.jobs.StartJob(JobMaker.MakeJob(RimWorld.JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
                                }

                            }
                        }
                    }
                    tickerInterval = 0;
                }
                tickerInterval++;
            }
            catch(NullReferenceException e)
            {

            }
        }
    }
    
}
