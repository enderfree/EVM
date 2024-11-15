using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Abilities
{
    public class ConsumeJoyPrey_AbilityEffect: CompProperties_AbilityEffect
    {
        public ConsumeJoyPrey_AbilityEffect()
        {
            this.compClass = typeof(ConsumeJoyPrey);
        }
    }
}
