﻿using System;
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
        public override void ApplyDigestion(SwallowWholeProperties swallowWholeProperties, ThingOwner innerContainer)
        {
            
        }

        public override float GetNutritionFromDigestion(SwallowWholeProperties swallowWholeProperties, ThingOwner innerContainer)
        {
            return 0f;
        }

        public override float GetNutritionFromDigestionTick(SwallowWholeProperties swallowWholeProperties, ThingOwner innerContainer)
        {
            return 0f;
        }

        public override void Struggle(Thing struggler, SwallowWholeProperties swallowWholeProperties)
        {

        }
    }
}
