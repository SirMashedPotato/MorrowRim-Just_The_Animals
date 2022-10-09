using Verse;
using RimWorld;
using System.Collections.Generic;

namespace MorrowRim
{
    [StaticConstructorOnStartup]
    public class VFIChitinIntegration
    {
        private static readonly List<ThingDef> toPatch = new List<ThingDef>() 
        {   
            ThingDefOf.MorrowRim_AshHopper, 
            ThingDefOf.MorrowRim_KwamaScrib, 
            ThingDefOf.MorrowRim_KwamaWarrior, 
            ThingDefOf.MorrowRim_KwamaWorker, 
            ThingDefOf.MorrowRim_NixOx, 
            ThingDefOf.MorrowRim_Shalk,
            ThingDefOf.MorrowRim_FungalShalk
        };

        static VFIChitinIntegration()
        {
            if (ModSettings_Utility.CheckVFEInsects() && ModSettings_Utility.MorrowRim_SettingVFIChitinIntegration())
            {
                foreach(ThingDef thing in toPatch)
                {
                    float f = thing.GetStatValueAbstract(StatDefOf.MeatAmount) / 2;
                    thing.SetStatBaseValue(StatDefOf.LeatherAmount, f);
                    SwitchChitin(thing, "VFEI_Chitin");
                }
            }
        }

        public static void SwitchChitin(ThingDef target, string newChitin)
        {
            ThingDef newChitinDef = DefDatabase<ThingDef>.GetNamed(newChitin);
            if (newChitinDef != null) target.race.leatherDef = newChitinDef;
        }
    }
}
