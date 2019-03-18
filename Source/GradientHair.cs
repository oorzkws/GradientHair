﻿using Harmony;
using System;
using System.Reflection;
using UnityEngine;
using Verse;
using GradientHair.Patch;

namespace GradientHair
{
    public class GradientHair : Mod
    {
        public static GradientHairModSettings settings;

        public GradientHair(ModContentPack pack) : base(pack)
        {
            var harmony = HarmonyInstance.Create("com.github.automatic1111.gradienthair");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Type panelAppearance = GenTypes.GetTypeInAnyAssembly("EdB.PrepareCarefully.PanelAppearance");
            if (panelAppearance != null)
                harmony.Patch(AccessTools.Method(panelAppearance, "DrawColorSelectorForPawnLayer"), null, new HarmonyMethod(typeof(PanelAppearanceDrawColorSelectorForPawnLayer), "Postfix"));
            
            settings = GetSettings<GradientHairModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("SettingEnableRandomName".Translate(), ref settings.enable, "SettingEnableRandomDesc".Translate());
            listing_Standard.SliderLabeled("SettingChanceMaleName".Translate(), ref settings.chanceMale, "SettingChanceMaleDesc".Translate(), 0, 1, settings.chanceMale.ToStringPercent());
            listing_Standard.SliderLabeled("SettingChanceFemaleName".Translate(), ref settings.chanceFemale, "SettingChanceFemaleDesc".Translate(), 0, 1, settings.chanceFemale.ToStringPercent());
            listing_Standard.End();
        }

        public override string SettingsCategory()
        {
            return "GradientHairTitle".Translate();
        }
    }
}
