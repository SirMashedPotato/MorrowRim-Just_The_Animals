using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Reflection;
using Verse;
using System;
using System.Linq;
using System.Collections.Generic;
using Verse.AI;
using Verse.AI.Group;
using System.Text;

namespace MorrowRim
{
    class HediffComp_Corprus : HediffComp
    {

        public HediffCompProperties_Corprus Props
        {
            get
            {
                return (HediffCompProperties_Corprus)this.props;
            }
        }

        static readonly String[] immuneDefs = { PawnKindDefOf.MorrowRim_CorprusLame.defName, PawnKindDefOf.MorrowRim_CorprusStalker.defName, PawnKindDefOf.MorrowRim_CorprusBloated.defName,
        "Dagoth_Slave", "Dagoth_Zombie", "Dagoth_Ghoul", "Dagoth_Vampire", "Dagoth_Ascended"};

        public override void CompPostTick(ref float severityAdjustment)
        {

            if(parent.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_CorprusSurvived, false) != null || !parent.pawn.RaceProps.IsFlesh || immuneDefs.Contains(parent.pawn.def.defName))
            {
                parent.pawn.health.RemoveHediff(parent.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_Corprus, false));
            }

            if (parent.Severity + severityAdjustment >= 1.0 && !parent.FullyImmune())
            {
                //If not treated in time and at 100% buildup
                Pawn p = parent.pawn;

                if (p.AnimalOrWildMan())
                {
                    p.Kill(null);
                }

                //check if pawn randomly survives
                if(Rand.Range(1,100) >= 99)
                {
                    //pawn somehow managed to survive and is buffed slightly
                    p.health.RemoveHediff(p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_Corprus, false));
                    p.health.AddHediff(HediffDefOf.MorrowRim_CorprusSurvived);
                    return;
                }

                //check if pawn is actually spawned in, then just kill them
                if (!p.Spawned || p.IsCaravanMember())
                {
                    bool flag = PawnUtility.ShouldSendNotificationAbout(p);
                    Caravan caravan = p.GetCaravan();
                    p.Kill(null, null);
                    if (flag)
                    {
                        p.health.NotifyPlayerOfKilled(null, this.parent, caravan);
                    }
                    return;
                }

                //drop items and such
                p.DropAndForbidEverything();
                p.Strip();
                p.health.RemoveHediff(p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_Corprus, false));
                //kill and spawn new corprus
                p.Kill(null);

                //roll to see what is spawned
                PawnKindDef corprus;
                int temp = Rand.RangeInclusive(1, 10);

                switch (temp)
                {
                    case 10:
                        corprus = PawnKindDefOf.MorrowRim_CorprusLame;
                        break;
                    case 9:
                        corprus = PawnKindDefOf.MorrowRim_CorprusBloated;
                        break;
                    default:
                        corprus = PawnKindDefOf.MorrowRim_CorprusStalker;
                        break;
                }

                Pawn newP = PawnGenerator.GeneratePawn(corprus, null);
                //GenSpawn.Spawn(newP, p.Corpse.Position, p.Map, Rot4.Random, WipeMode.Vanish, false);
                PawnUtility.TrySpawnHatchedOrBornPawn(newP, p.Corpse);
                newP.gender = p.gender;
                newP.Name = p.Name;
                //newP.health.Reset();
                //newP.health.AddHediff(HediffDefOf.MorrowRim_CorprusPermanent);
                //newP.SetFaction(FactionUtility.DefaultFactionFrom(FactionDefOf.MorrowRim_Corprus));
                newP.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);

                p.Corpse.Destroy();
                return;
            }
            base.CompPostTick(ref severityAdjustment);
        }
    }

}
