using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    class DeathActionWorker_BloatedCorprus : DeathActionWorker
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
            //Need to spawn gas
            GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Burn, corpse.InnerPawn, -1, -1f, null, null, null, null, ThingDefOf.Gas_Corprus, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
            FilthMaker.TryMakeFilth(corpse.Position, corpse.Map, RimWorld.ThingDefOf.Filth_CorpseBile, 3);
            if (!corpse.Destroyed)
            {
                corpse.Destroy();
            }
        }

    }
}
