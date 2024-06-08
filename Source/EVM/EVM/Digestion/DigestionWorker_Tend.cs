using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Digestion
{
    public class DigestionWorker_Tend : DigestionWorker
    {
        public virtual float Heal(Pawn pawn, float healingPool)
        {
            while (healingPool > 0f && pawn.health.hediffSet.GetHediffsTendable().Count<Hediff>() > 0)
            {
                float tendQuality = 0f;

                if (healingPool > 1f)
                {
                    tendQuality = 1f;
                    healingPool -= 1f;
                }
                else
                {
                    tendQuality = healingPool;
                    healingPool = 0f;
                }

                pawn.health.hediffSet.GetHediffsTendable().RandomElement<Hediff>().Tended(tendQuality, 1f, 1);
            }

            return healingPool;
        }

        public override void ApplyDigestion(VoreProperties voreProperties, ThingOwner innerContainer)
        {
            float healingPool = voreProperties.baseDamage * base.GetDigestionEfficiancy(voreProperties);

            if (healingPool > 0)
            {
                foreach (Thing thing in innerContainer)
                {
                    if (voreProperties.canDigest(thing))
                    {
                        if (thing is Pawn pawn)
                        {
                            Heal(pawn, healingPool);
                        }
                    }
                }
            }
        }

        public override float GetNutritionFromDigestionTick(VoreProperties voreProperties, ThingOwner innerContainer)
        {
            float healingPool = voreProperties.baseDamage * base.GetDigestionEfficiancy(voreProperties);
            allHealingSoFar += healingPool;

            return healingPool;
        }

        public override float GetNutritionFromDigestion(VoreProperties voreProperties, ThingOwner innerContainer)
        {
            float nutrition = 0f;

            if (EnderfreesVoreMod.settings.nutritionGainOption == (int)NutritionGainOptions.OnEating)
            {
                foreach (Thing thing in innerContainer)
                {
                    if (thing is Pawn pawn)
                    {
                        foreach (Hediff hediff in pawn.health.hediffSet.GetHediffsTendable())
                        {
                            ++nutrition;
                        }
                    }
                }
            }
            else if (EnderfreesVoreMod.settings.nutritionGainOption == (int)NutritionGainOptions.AfterDigestion)
            {
                nutrition = allHealingSoFar;
            }

            return nutrition;
        }

        public float allHealingSoFar = 0f;

        public override void ExposeData()
        {
            Scribe_Values.Look<float>(ref allHealingSoFar, "EVM_HealingVoreWorker_HealingPoolCount");
        }
    }
}
