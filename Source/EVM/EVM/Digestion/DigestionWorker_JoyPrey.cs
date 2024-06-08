using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Digestion
{
    public class DigestionWorker_JoyPrey : DigestionWorker
    {
        public override void ApplyDigestion(VoreProperties voreProperties, ThingOwner innerContainer)
        {
            foreach (Thing thing in innerContainer)
            {
                if (thing is Pawn pawn)
                {
                    Need_Joy joyNeed = pawn.needs?.TryGetNeed<Need_Joy>();

                    if (joyNeed != null) 
                    {
                        joyNeed.GainJoy(3.5E-05f * 530, JoyKindDefOf.Meditative);
                    }
                }
            }
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
