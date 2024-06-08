using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Digestion
{
    public class DigestionWorker_Safe : DigestionWorker
    {
        public override void ApplyDigestion(VoreProperties voreProperties, ThingOwner innerContainer)
        {
            
        }

        public override float GetNutritionFromDigestion(VoreProperties voreProperties, ThingOwner innerContainer)
        {
            return 0f;
        }

        public override float GetNutritionFromDigestionTick(VoreProperties voreProperties, ThingOwner innerContainer)
        {
            return 0f;
        }
    }
}
