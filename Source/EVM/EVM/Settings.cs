using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM
{
    internal class Settings: ModSettings
    {
        public int nutritionGainOption = (int)NutritionGainOptions.OnEating;
        public bool swallowIgnoresSize = false;
        public bool voreDebugOptions = false;

        public override void ExposeData()
        {
            Scribe_Values.Look<int>(ref nutritionGainOption, "EVM_NutritionGainOption");
            Scribe_Values.Look<bool>(ref swallowIgnoresSize, "EVM_SwallowIgnoresSize");
            Scribe_Values.Look<bool>(ref voreDebugOptions, "EVM_VoreDebugOptions");
        }
    }
}
