using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Release.DigestionWorkerSpecific
{
    public class CustomReleaseWorker_HealScars : CustomReleaseWorker
    {
        public override bool ShouldRelease(SwallowWholeProperties swallowWholeProperties)
        {
            CustomReleaseWorker_NoBedRestNeed customReleaseWorker_NoBedRestNeed = new CustomReleaseWorker_NoBedRestNeed();
            CustomReleaseWorker_Scarless customReleaseWorker_Scarless = new CustomReleaseWorker_Scarless();

            return customReleaseWorker_NoBedRestNeed.ShouldRelease(swallowWholeProperties) && 
                customReleaseWorker_Scarless.ShouldRelease(swallowWholeProperties);
        }
    }
}
