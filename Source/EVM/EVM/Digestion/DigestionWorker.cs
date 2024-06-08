using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Digestion
{
    public abstract class DigestionWorker: IExposable
    {
        public virtual float GetDigestionEfficiancy(VoreProperties voreProperties)
        {
            if (!voreProperties.pred.health.capacities.CapableOf(InternalDefOf.Metabolism))
            {
                return 0;
            }

            return voreProperties.digestionEfficiancy * voreProperties.pred.health.capacities.GetLevel(InternalDefOf.Metabolism);
        }

        public abstract void ApplyDigestion(VoreProperties voreProperties, ThingOwner innerContainer);

        public virtual void Struggle(Thing struggler, VoreProperties voreProperties)
        {
            if (struggler is Pawn pawn)
            {
                if (!(pawn.Downed || pawn.Dead))
                {
                    int meleeLvl = 0;

                    if (pawn.skills != null)
                    {
                        SkillRecord melee = pawn.skills.GetSkill(SkillDefOf.Melee);
                        if (melee != null)
                        {
                            if (meleeLvl < melee.Level)
                            {
                                meleeLvl = melee.Level;
                            }
                        }
                    }
                    
                    VerbEntry verbEntry = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false).RandomElement<VerbEntry>();
                    float damage = verbEntry.verb.verbProps.AdjustedMeleeDamageAmount(verbEntry.verb, pawn) * meleeLvl / 10 * voreProperties.pred.GetStatValue(StatDefOf.IncomingDamageFactor, true, -1);
                    DamageDef damageDef = verbEntry.verb.verbProps.meleeDamageDef;
                    
                    int rand = Enumerable.Range(1, 100).RandomElement<int>();
                    int armorValue = 0;
                    
                    if (voreProperties.armorValues.ContainsKey(damageDef.armorCategory))
                    {
                        armorValue = voreProperties.armorValues[damageDef.armorCategory];
                    }
                    
                    if (rand < armorValue / 2)
                    {
                        return;
                    }
                    else if (rand <= armorValue)
                    {
                        damage /= 2;
                    }
                    
                    voreProperties.pred.TakeDamage(new DamageInfo(
                            damageDef,
                            damage,
                            500f,
                            -1,
                            struggler,
                            voreProperties.pred.RaceProps.body.GetPartsWithDef(voreProperties.digestiveTracks[voreProperties.trackId].track[voreProperties.trackStage])[0],
                            null,
                            DamageInfo.SourceCategory.ThingOrUnknown,
                            voreProperties.pred,
                            false,
                            false
                        ));
                }
            }
        }

        public abstract float GetNutritionFromDigestionTick(VoreProperties voreProperties, ThingOwner innerContainer);
        
        public abstract float GetNutritionFromDigestion(VoreProperties voreProperties, ThingOwner innerContainer);

        public virtual void ExposeData()
        {
            
        }
    }
}
