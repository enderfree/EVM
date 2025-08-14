using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using EVM.Digestion;

namespace EVM.Release
{
    public class CustomReleaseWorker_Healed : CustomReleaseWorker
    {
        public override bool ShouldRelease(SwallowWholeProperties swallowWholeProperties)
        {
            if (swallowWholeProperties.prey is Pawn prey)
            {
                return new DigestionWorker_Heal().GetHealables(prey).Count<Hediff>() <= 0;
            }

            return true;
        }
    }
}
