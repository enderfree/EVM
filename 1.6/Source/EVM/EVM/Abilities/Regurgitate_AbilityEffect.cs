using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;

namespace EVM.Abilities
{
    public class Regurgitate_AbilityEffect: CompProperties_AbilityEffect
    {
        public Regurgitate_AbilityEffect() 
        {
            this.compClass = typeof(Regurgitate);
        }
    }
}
