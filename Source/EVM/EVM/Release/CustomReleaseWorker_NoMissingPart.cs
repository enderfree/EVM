using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Release
{
    public class CustomReleaseWorker_NoMissingPart : CustomReleaseWorker
    {
        public override bool ShouldRelease(SwallowWholeProperties swallowWholeProperties)
        {
            if (swallowWholeProperties.prey is Pawn prey)
            {
                return !prey.health.hediffSet.HasHediff<Hediff_MissingPart>();
            }

            return true;
        }
    }
}
