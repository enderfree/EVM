using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;

namespace EVM.Abilities
{
    public class ConsumeFatal_AbilityEffect: CompProperties_AbilityEffect
    {
        public ConsumeFatal_AbilityEffect()
        {
            this.compClass = typeof(ConsumeFatal);
        }
    }
}
