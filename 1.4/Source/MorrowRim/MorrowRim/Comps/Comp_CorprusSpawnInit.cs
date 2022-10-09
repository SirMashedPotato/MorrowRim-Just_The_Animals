using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace MorrowRim
{
    class Comp_CorprusSpawnInit : ThingComp
    {
        private bool doneAlready = false;

        public void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.doneAlready, "summonOnce", true, false);
        }

        public override void CompTick()
        {
            base.CompTick();

            if (!doneAlready)
            {
                Pawn p = parent as Pawn;
                p.health.Reset();
                p.health.AddHediff(HediffDefOf.MorrowRim_CorprusPermanent);

                if(p.Name == null)
                {
                    p.Name = PawnBioAndNameGenerator.GeneratePawnName(p, NameStyle.Full);
                }
                if(p.Faction == null)
                {
                    p.SetFaction(FactionUtility.DefaultFactionFrom(FactionDefOf.MorrowRim_Corprus));
                }

                doneAlready = true;
            }
        }
    }
}
