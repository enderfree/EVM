using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Release.DigestionWorkerSpecific
{
    public class CustomReleaseWorker_Regenerate : CustomReleaseWorker
    {
        public override bool ShouldRelease(SwallowWholeProperties swallowWholeProperties)
        {
            CustomReleaseWorker_HealScars customReleaseWorker_HealScars = new CustomReleaseWorker_HealScars();
            CustomReleaseWorker_NoMissingPart customReleaseWorker_NoMissingPart = new CustomReleaseWorker_NoMissingPart();

            return customReleaseWorker_HealScars.ShouldRelease(swallowWholeProperties) &&
                customReleaseWorker_NoMissingPart.ShouldRelease(swallowWholeProperties);
        }
    }
}
