using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Release
{
    public class CustomReleaseWorker_Scarless : CustomReleaseWorker
    {
        public override bool ShouldRelease(SwallowWholeProperties swallowWholeProperties)
        {
            if (swallowWholeProperties.prey is Pawn prey)
            {
                foreach (Hediff hediff in prey.health.hediffSet.hediffs)
                {
                    if (hediff is Hediff_Injury && hediff.Severity > 0 && hediff.IsPermanent())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
