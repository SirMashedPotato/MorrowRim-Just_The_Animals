using RimWorld;
using Verse;
using Verse.Sound;

namespace MorrowRim
{
    static class ModSettings_Presets
    {
        static readonly SoundDef sound = RimWorld.SoundDefOf.TinyBell;

        public static void PresetResetSettings(MorrowRim_ModSettings settings)
        {
            MorrowRim_ModSettings.ResetSettings_Ash(settings);
            MorrowRim_ModSettings.ResetSettings_Blight(settings);
            MorrowRim_ModSettings.ResetSettings_Fire(settings);
            MorrowRim_ModSettings.ResetSettings_Incident(settings);
            MorrowRim_ModSettings.ResetSettings_Zombieland(settings);
            sound.PlayOneShotOnCamera();
        }

        public static void PresetSettings_VolcanicHellscape(MorrowRim_ModSettings settings)
        {
            PresetResetSettings(settings);
            //ash
            settings.MorrowRim_SettingEnableLightingEffects = true;
            settings.MorrowRim_SettingEnableAshEffects = true;
            settings.MorrowRim_SettingAshFilledEye = 75;
            settings.MorrowRim_SettingAshCloggedServo = 25;
            settings.MorrowRim_SettingAshIgnoreResistance = true;
            settings.MorrowRim_SettingAshBuildupMultiplier = 3;
            settings.MorrowRim_SettingAshPlantDamage = 5;
            settings.MorrowRim_SettingAshPlantChance = 75;
            settings.MorrowRim_SettingAshPreventVisitors = true;
            settings.MorrowRim_SettingAshRegrowth = false;
            settings.MorrowRim_SettingAshTurbineDamage = 3;
            //blight
            settings.MorrowRim_SettingEnableBlightEffects = true;
            settings.MorrowRim_SettingBlightPlantChance = 25;
            settings.MorrowRim_SettingBlightAnimalChance = 25;
            settings.MorrowRim_SettingBlightAnimalIgnoreScaling = true;
            settings.MorrowRim_SettingBlightAnimalNumber = 3;
            settings.MorrowRim_SettingBlightPlantNumber = 10;
            //fire
            settings.MorrowRim_SettingEnableFireEffects = true;
            settings.MorrowRim_SettingFireOnlySownPlants = false;
            settings.MorrowRim_SettingFireBurnChance = 20;
            settings.MorrowRim_SettingFireFirePawnChance = 25;
            settings.MorrowRim_SettingFireFirePlantChance = 10;
            //other
            settings.MorrowRim_SettingEnableSiltStriderEvent = false;
            settings.MorrowRim_SettingEnableSiltStriderExtinction = true;
            settings.MorrowRim_SettingZombielandIntegration = false;
        }

        public static void PresetSettings_AdaptedAshlands(MorrowRim_ModSettings settings)
        {
            PresetResetSettings(settings);
            //ash
            settings.MorrowRim_SettingAshOnlySownPlants = true;
            settings.MorrowRim_SettingAshRegrowth = true;
            settings.MorrowRim_SettingAshPlantDamage = 4;
            settings.MorrowRim_SettingAshPlantChance = 75;
            //fire
            settings.MorrowRim_SettingFireOnlySownPlants = true;
            settings.MorrowRim_SettingFireBurnChance = 15;
            settings.MorrowRim_SettingFireFirePlantChance = 5;
        }

        public static void PresetSettings_Peaceful(MorrowRim_ModSettings settings)
        {
            PresetResetSettings(settings);
            //ash
            settings.MorrowRim_SettingEnableLightingEffects = true;
            settings.MorrowRim_SettingEnableAshEffects = false;
            settings.MorrowRim_SettingAshPreventVisitors = false;
            //blight
            settings.MorrowRim_SettingEnableBlightEffects = false;
            //fire
            settings.MorrowRim_SettingEnableFireEffects = false;
            //other
            settings.MorrowRim_SettingEnableKwamaTrojanInfestation = false;
            settings.MorrowRim_SettingEnableCliffRacerEvents = false;
            settings.MorrowRim_SettingEnableCliffRacerExtinction = true;
            settings.MorrowRim_SettingEnableCorprusExtinction = true;
        }

        public static void PresetSettings_DagothDead(MorrowRim_ModSettings settings)
        {
            PresetResetSettings(settings);
            //blight
            settings.MorrowRim_SettingEnableBlightEffects = false;
            //other
            settings.MorrowRim_SettingEnableCorprusExtinction = true;
        }

        public static void PresetSettings_Zombieland(MorrowRim_ModSettings settings)
        {
            PresetResetSettings(settings);
            //ash
            settings.MorrowRim_SettingAshOnlySownPlants = true;
            settings.MorrowRim_SettingAshRegrowth = true;
            settings.MorrowRim_SettingAshPlantDamage = 3;
            settings.MorrowRim_SettingAshPlantChance = 60;
            settings.MorrowRim_SettingAshBuildupMultiplier = 1.5f;
            settings.MorrowRim_SettingAshPreventVisitors = true;
            //fire
            settings.MorrowRim_SettingFireOnlySownPlants = true;
            settings.MorrowRim_SettingFireBurnChance = 15;
            settings.MorrowRim_SettingFireFirePlantChance = 5;
            //other
            settings.MorrowRim_SettingEnableCorprusExtinction = true;
            settings.MorrowRim_SettingEnableCliffRacerExtinction = true;
            settings.MorrowRim_SettingEnableCliffRacerEvents = false;
            settings.MorrowRim_SettingZombielandIntegration = true;
        }
    }
}
