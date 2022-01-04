using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    public class DeathActionWorker_RetchingNetch : DeathActionWorker
    {

        public override RulePackDef DeathRules
        {
            get
            {
                return RulePackDefOf.Transition_DiedExplosive;
            }
        }

        public override bool DangerousInMelee
        {
            get
            {
                return true;
            }
        }

        public override void PawnDied(Corpse corpse)
        {
            if (corpse.InnerPawn.ageTracker.CurLifeStageIndex != 0) //only explodes if not bebe
            {
                GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, ThingDefOf.Gas_RetchingNetch, 1f, 1, false, null, 0f, 1, 0f, false, null, null);

            }
        }

    }
}
