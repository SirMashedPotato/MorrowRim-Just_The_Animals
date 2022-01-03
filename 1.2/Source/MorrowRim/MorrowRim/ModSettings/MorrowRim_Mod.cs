using UnityEngine;
using Verse;
using System;

namespace MorrowRim
{
    class MorrowRim_Mod : Mod
    {
        MorrowRim_ModSettings settings;
        public MorrowRim_Mod(ModContentPack contentPack) : base(contentPack)
        {
            this.settings = GetSettings<MorrowRim_ModSettings>();
        }

        public override string SettingsCategory() => "MorrowRim - Just The Animals";

        private int page = 0;

        private static Vector2 scrollPosition = Vector2.zero;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            Rect rect2 = new Rect(0f, 0f, inRect.width - 30, inRect.height * 2);
            Widgets.BeginScrollView(rect, ref scrollPosition, rect2);
            listing_Standard.Begin(rect2);

            //get page
            switch (page)
            {
                case 1:
                    listing_Standard = SettingsPage_Incident(listing_Standard);
                    break;

                case 2:
                    listing_Standard = SettingsPage_AnimalBehaviour(listing_Standard);
                    break;

                case 3:
                    listing_Standard = SettingsPage_ModIntegration(listing_Standard);
                    break;

                default:
                    listing_Standard = SettingsPage_Default(listing_Standard);
                    break;
            }

            listing_Standard = SettingsButtons(listing_Standard);

            listing_Standard.End();
            Widgets.EndScrollView();
            base.DoSettingsWindowContents(inRect);
        }

        private Listing_Standard SettingsButtons(Listing_Standard listing_Standard)
        {
            listing_Standard.Gap();
            listing_Standard.GapLine();
            //pages

            if (page != 0)
            {
                Rect rectPageDefault = listing_Standard.GetRect(30f);
                TooltipHandler.TipRegion(rectPageDefault, "MorrowRim_PageDefault".Translate());
                if (Widgets.ButtonText(rectPageDefault, "MorrowRim_PageDefault".Translate(), true, true, true))
                {
                    page = 0;
                }
                listing_Standard.Gap();
            }

            if (page != 1)
            {

                Rect rectPageIncident = listing_Standard.GetRect(30f);
                TooltipHandler.TipRegion(rectPageIncident, "MorrowRim_PageIncident".Translate());
                if (Widgets.ButtonText(rectPageIncident, "MorrowRim_PageIncident".Translate(), true, true, true))
                {
                    page = 1;
                }
                listing_Standard.Gap();
            }

            if (page != 2)
            {

                Rect rectPageAnimalBehaviour = listing_Standard.GetRect(30f);
                TooltipHandler.TipRegion(rectPageAnimalBehaviour, "MorrowRim_PageAnimalBehaviour".Translate());
                if (Widgets.ButtonText(rectPageAnimalBehaviour, "MorrowRim_PageAnimalBehaviour".Translate(), true, true, true))
                {
                    page = 2;
                }
                listing_Standard.Gap();
            }

            if (page != 3)
            {

                Rect rectPageModIntegration = listing_Standard.GetRect(30f);
                TooltipHandler.TipRegion(rectPageModIntegration, "MorrowRim_PageModIntegration".Translate());
                if (Widgets.ButtonText(rectPageModIntegration, "MorrowRim_PageModIntegration".Translate(), true, true, true))
                {
                    page = 3;
                }
                listing_Standard.Gap();
            }

            listing_Standard.GapLine();

            //reset
            Rect rectDefault = listing_Standard.GetRect(30f);
            TooltipHandler.TipRegion(rectDefault, "MorrowRim_ResetDefault".Translate());
            if (Widgets.ButtonText(rectDefault, "MorrowRim_ResetDefault".Translate(), true, true, true))
            {
                MorrowRim_ModSettings.ResetSettings(settings);
            }

            return listing_Standard;
        }

        //Specific pages

        private Listing_Standard SettingsPage_Default(Listing_Standard listing_Standard)
        {
            listing_Standard.Label("MorrowRim_PageDefault".Translate());
            listing_Standard.GapLine();
            listing_Standard.Gap();


            return listing_Standard;
        }

        private Listing_Standard SettingsPage_Incident(Listing_Standard listing_Standard)
        {
            listing_Standard.Label("MorrowRim_PageIncident".Translate());
            listing_Standard.GapLine();
            listing_Standard.Gap();

            //settings
            listing_Standard.CheckboxLabeled("MorrowRim_SettingEnableAlbinoGuarEvent".Translate(), ref settings.MorrowRim_SettingEnableAlbinoGuarEvent);
            listing_Standard.Gap();

            listing_Standard.CheckboxLabeled("MorrowRim_SettingEnableCliffRacerEvents".Translate(), ref settings.MorrowRim_SettingEnableCliffRacerEvents);
            listing_Standard.Gap();

            listing_Standard.CheckboxLabeled("MorrowRim_SettingEnableSiltStriderEvent".Translate(), ref settings.MorrowRim_SettingEnableSiltStriderEvent);
            listing_Standard.Gap();

            //reset
            Rect rectDefault = listing_Standard.GetRect(30f);
            TooltipHandler.TipRegion(rectDefault, "MorrowRim_ResetIncident".Translate());
            if (Widgets.ButtonText(rectDefault, "MorrowRim_ResetIncident".Translate(), true, true, true))
            {
                MorrowRim_ModSettings.ResetSettings_Incident(settings);
            }

            return listing_Standard;
        }

        private Listing_Standard SettingsPage_ModIntegration(Listing_Standard listing_Standard)
        {
            listing_Standard.Label("MorrowRim_PageModIntegration".Translate());
            listing_Standard.GapLine();
            listing_Standard.Label("MorrowRim_ModIntegrationText1".Translate());
            listing_Standard.Label("MorrowRim_ModIntegrationText2".Translate());
            listing_Standard.GapLine();
            listing_Standard.Gap();

            //settings
            if (ModSettings_Utility.CheckVFEInsects())
            {
                listing_Standard.CheckboxLabeled("MorrowRim_SettingVFIChitinIntegration".Translate(), ref settings.MorrowRim_SettingVFIChitinIntegration);
                listing_Standard.Gap();
            }

            //reset
            Rect rectDefault = listing_Standard.GetRect(30f);
            TooltipHandler.TipRegion(rectDefault, "MorrowRim_ResetModIntegration".Translate());
            if (Widgets.ButtonText(rectDefault, "MorrowRim_ResetModIntegration".Translate(), true, true, true))
            {
                MorrowRim_ModSettings.ResetSettings_ModIntegration(settings);
            }

            return listing_Standard;
        }

        private Listing_Standard SettingsPage_AnimalBehaviour(Listing_Standard listing_Standard)
        {
            listing_Standard.Label("MorrowRim_PageAnimalBehaviour".Translate());
            listing_Standard.GapLine();
            listing_Standard.Gap();

            //settings
            //listing_Standard.CheckboxLabeled("MorrowRim_SettingEnableScampBehaviour".Translate(), ref settings.MorrowRim_SettingEnableScampBehaviour);
            //listing_Standard.Gap();

            listing_Standard.CheckboxLabeled("MorrowRim_SettingEnableScampInsults".Translate(), ref settings.MorrowRim_SettingEnableScampInsults);
            listing_Standard.Gap();

            //reset
            Rect rectDefault = listing_Standard.GetRect(30f);
            TooltipHandler.TipRegion(rectDefault, "MorrowRim_ResetAnimalBehaviour".Translate());
            if (Widgets.ButtonText(rectDefault, "MorrowRim_ResetAnimalBehaviour".Translate(), true, true, true))
            {
                MorrowRim_ModSettings.ResetSettings_AnimalBehaviour(settings);
            }

            return listing_Standard;
        }

    }
}
