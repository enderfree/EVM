using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVM
{
    internal enum NutritionGainOptions
    {
        [Description("On Eating")]
        OnEating,
        [Description("Per Digestion Tick")]
        PerDigestionTick,
        [Description("After Digestion (not implemented)")]
        AfterDigestion
    }
}
