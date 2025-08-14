using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Abilities
{
    public class ConsumeHealScars_AbilityEffect: CompProperties_AbilityEffect
    {
        public ConsumeHealScars_AbilityEffect()
        {
            this.compClass = typeof(ConsumeHealScars);
        }
    }
}
