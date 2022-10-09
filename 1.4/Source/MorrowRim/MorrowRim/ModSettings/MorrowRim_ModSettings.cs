using Verse;

namespace MorrowRim
{
    class MorrowRim_ModSettings : ModSettings
    {
        //settings
    
        public bool MorrowRim_SettingEnableCliffRacerEvents = MorrowRim_SettingEnableCliffRacerEvents_def;
        public bool MorrowRim_SettingEnableSiltStriderEvent = MorrowRim_SettingEnableSiltStriderEvent_def;
        public bool MorrowRim_SettingEnableAlbinoGuarEvent = MorrowRim_SettingEnableAlbinoGuarEvent_def;

        public bool MorrowRim_SettingEnableScampBehaviour = MorrowRim_SettingEnableScampBehaviour_def;
        public bool MorrowRim_SettingEnableScampInsults = MorrowRim_SettingEnableScampInsults_def;

        public bool MorrowRim_SettingVFIChitinIntegration = MorrowRim_SettingVFIChitinIntegration_def;


        //defaults
        private static readonly bool MorrowRim_SettingEnableCliffRacerEvents_def = true;
        private static readonly bool MorrowRim_SettingEnableAlbinoGuarEvent_def = true;
        private static readonly bool MorrowRim_SettingEnableSiltStriderEvent_def = true;

        private static readonly bool MorrowRim_SettingEnableScampBehaviour_def = true;
        private static readonly bool MorrowRim_SettingEnableScampInsults_def = true;

        private static readonly bool MorrowRim_SettingVFIChitinIntegration_def = false;

        //save settings
        public override void ExposeData()
        {
            Scribe_Values.Look(ref MorrowRim_SettingEnableCliffRacerEvents, "MorrowRim_SettingEnableCliffRacerEvents", MorrowRim_SettingEnableCliffRacerEvents_def);
            Scribe_Values.Look(ref MorrowRim_SettingEnableAlbinoGuarEvent, "MorrowRim_SettingEnableAlbinoGuarEvent", MorrowRim_SettingEnableAlbinoGuarEvent_def);
            Scribe_Values.Look(ref MorrowRim_SettingEnableSiltStriderEvent, "MorrowRim_SettingEnableSiltStriderEvent", MorrowRim_SettingEnableSiltStriderEvent_def);

            Scribe_Values.Look(ref MorrowRim_SettingEnableScampBehaviour, "MorrowRim_SettingEnableScampBehaviour", MorrowRim_SettingEnableScampBehaviour_def);
            Scribe_Values.Look(ref MorrowRim_SettingEnableScampInsults, "MorrowRim_SettingEnableScampInsults", MorrowRim_SettingEnableScampInsults_def);

            Scribe_Values.Look(ref MorrowRim_SettingVFIChitinIntegration, "MorrowRim_SettingVFIChitinIntegration", MorrowRim_SettingVFIChitinIntegration_def);

            base.ExposeData();
        }

        //reset settings

        public static void ResetSettings(MorrowRim_ModSettings settings)
        {
            ResetSettings_Incident(settings);
            ResetSettings_ModIntegration(settings);
            ResetSettings_AnimalBehaviour(settings);
        }

        public static void ResetSettings_Incident(MorrowRim_ModSettings settings)
        {
            settings.MorrowRim_SettingEnableCliffRacerEvents = MorrowRim_SettingEnableCliffRacerEvents_def;
            settings.MorrowRim_SettingEnableAlbinoGuarEvent = MorrowRim_SettingEnableAlbinoGuarEvent_def;
            settings.MorrowRim_SettingEnableSiltStriderEvent = MorrowRim_SettingEnableSiltStriderEvent_def;
        }

        public static void ResetSettings_AnimalBehaviour(MorrowRim_ModSettings settings)
        {
            settings.MorrowRim_SettingEnableScampBehaviour = MorrowRim_SettingEnableScampBehaviour_def;
            settings.MorrowRim_SettingEnableScampInsults = MorrowRim_SettingEnableScampInsults_def;
        }

        public static void ResetSettings_ModIntegration(MorrowRim_ModSettings settings)
        {
            settings.MorrowRim_SettingVFIChitinIntegration = MorrowRim_SettingVFIChitinIntegration_def;
        }
    }
}
