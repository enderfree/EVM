using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Digestion
{
    public class DigestionWorker_Fatal : DigestionWorker
    {

        public override void ApplyDigestion(SwallowWholeProperties swallowWholeProperties, ThingOwner innerContainer)
        {
            float digestionEfficiancy = base.GetDigestionEfficiancy(swallowWholeProperties);
            
            if (digestionEfficiancy > 0)
            {
                // digest
                float damage = swallowWholeProperties.baseDamage * digestionEfficiancy;

                foreach (Thing thing in innerContainer)
                {
                    if (swallowWholeProperties.canDigest(thing))
                    {
                        thing.TakeDamage(new DamageInfo(
                            swallowWholeProperties.digestionDamageType,
                            damage,
                            100f,
                            -1f,
                            swallowWholeProperties.pred,
                            null,
                            null,
                            DamageInfo.SourceCategory.ThingOrUnknown,
                            thing,
                            true,
                            false
                        ));
                    }
                }
            }
        }

        public override float GetNutritionFromDigestion(SwallowWholeProperties swallowWholeProperties, ThingOwner innerContainer)
        {
            float nutrition = 0f;

            foreach (Thing thing in innerContainer)
            {
                if (!swallowWholeProperties.canDigest(thing))
                {
                    Corpse corpse = null;

                    if (thing is Corpse maybe)
                    {
                        corpse = maybe;
                    }
                    else if (thing is Pawn pawn)
                    {
                        corpse = pawn.Corpse;
                    }

                    float thisNutrition = 0.9f;

                    try
                    {
                        thisNutrition = thing.GetStatValue(StatDefOf.Nutrition);
                    }
                    catch
                    {
                        // may not exist
                    }

                    nutrition += thisNutrition;
                }
            }

            return nutrition;
        }

        public override float GetNutritionFromDigestionTick(SwallowWholeProperties voreProperties, ThingOwner innerContainer)
        {
            float nutrition = 0f;

            foreach (Thing thing in innerContainer)
            {
                if (voreProperties.canDigest(thing))
                {
                    float factor = 0f;

                    try
                    {
                        factor = thing.HitPoints / thing.MaxHitPoints;
                    }
                    catch
                    {
                        // thing may not use this hp system
                    }

                    Corpse corpse = null;

                    if (thing is Corpse maybe)
                    {
                        corpse = maybe;
                    }
                    else if (thing is Pawn pawn)
                    {
                        corpse = pawn.Corpse;
                        factor = pawn.health.summaryHealth.SummaryHealthPercent;
                    }

                    float totalNutrition = 0.9f;

                    try
                    {
                        totalNutrition = thing.GetStatValue(StatDefOf.Nutrition);
                    }
                    catch
                    {
                        // may not exist
                    }

                    nutrition += totalNutrition * factor;
                }
            }

            return nutrition;
        }
    }
}
