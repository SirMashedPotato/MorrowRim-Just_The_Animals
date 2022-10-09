using Verse;

namespace MorrowRim
{
    class ModSettings_Utility
    {

        public static float SettingToFloat(int setting)
        {
            float f = setting;
            return f / 100;
        }

        /* incidents */

        public static bool MorrowRim_SettingEnableCliffRacerEvents()
        {
            return LoadedModManager.GetMod<MorrowRim_Mod>().GetSettings<MorrowRim_ModSettings>().MorrowRim_SettingEnableCliffRacerEvents;
        }

        public static bool MorrowRim_SettingEnableSiltStriderEvent()
        {
            return LoadedModManager.GetMod<MorrowRim_Mod>().GetSettings<MorrowRim_ModSettings>().MorrowRim_SettingEnableSiltStriderEvent;
        }

        public static bool MorrowRim_SettingEnableAlbinoGuarEvent()
        {
            return LoadedModManager.GetMod<MorrowRim_Mod>().GetSettings<MorrowRim_ModSettings>().MorrowRim_SettingEnableAlbinoGuarEvent;
        }

        /* animal behaviour */

        public static bool MorrowRim_SettingEnableScampBehaviour()
        {
            return LoadedModManager.GetMod<MorrowRim_Mod>().GetSettings<MorrowRim_ModSettings>().MorrowRim_SettingEnableScampBehaviour;
        }

        public static bool MorrowRim_SettingEnableScampInsults()
        {
            return LoadedModManager.GetMod<MorrowRim_Mod>().GetSettings<MorrowRim_ModSettings>().MorrowRim_SettingEnableScampInsults;
        }

        /* mod integration */
        public static bool CheckVFEInsects()
        {
            return LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Factions Expanded - Insectoids");
        }

        public static bool MorrowRim_SettingVFIChitinIntegration()
        {
            return LoadedModManager.GetMod<MorrowRim_Mod>().GetSettings<MorrowRim_ModSettings>().MorrowRim_SettingVFIChitinIntegration;
        }
    }
}
