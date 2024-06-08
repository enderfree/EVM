using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;

namespace EVM
{
    internal class EnderfreesVoreMod: Mod
    {
        private static Vector2 scrollPosition = Vector2.zero;

        public static Settings settings;

        public EnderfreesVoreMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            NutritionGainOptions[] nutritionGainOptions = (NutritionGainOptions[])Enum.GetValues(typeof(NutritionGainOptions));

            Listing_Standard listing_Standard = new Listing_Standard();
            Rect outRect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            Rect viewRect = new Rect(0f, 0f, inRect.width - 30f, nutritionGainOptions.Length * 24 + 24 * 4);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            listing_Standard.Begin(viewRect);

            //listing_Standard.BeginSection(nutritionOptionsArray.Length * 24 + 24);

            listing_Standard.Label("Nutrition Gain Moment");
            foreach (NutritionGainOptions nutritionGainOption in nutritionGainOptions)
            {
                listing_Standard.RadioButton(nutritionGainOption.GetDescription(), settings.nutritionGainOption == (int)nutritionGainOption);
            }

            //listing_Standard.EndSection(listing_Standard);

            listing_Standard.CheckboxLabeled("Swallow Ignores Size", ref settings.swallowIgnoresSize, 1f);
            listing_Standard.CheckboxLabeled("Vore Debug Options", ref settings.voreDebugOptions, 1f);

            listing_Standard.End();
            Widgets.EndScrollView();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Enderfree's Vore Mod";
        }
    }
}
