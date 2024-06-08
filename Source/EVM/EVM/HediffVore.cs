using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using EVM.Digestion;

namespace EVM
{
    public class HediffVore: HediffWithComps, IThingHolder
    {
        public override bool TryMergeWith(Hediff other)
        {
            return false;
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
            remainingStageTime = voreProperties.deadline;

            if (voreProperties.pred == null)
            {
                voreProperties.pred = this.pawn;
            }

            if (EnderfreesVoreMod.settings.nutritionGainOption == (int)NutritionGainOptions.OnEating)
            {
                voreProperties.digestionWorker.GetNutritionFromDigestion(voreProperties, innerContainer);
            }
        }

        public void FreePreys()
        {
            foreach (Thing thing in innerContainer)
            {
                GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.MapHeld, ThingPlaceMode.Near, null, null, default(Rot4));
            }
        }

        public override void Tick()
        {
            base.Tick();
            
            if (Find.TickManager.TicksGame % 530 == 0)
            {
                // digestion
                voreProperties.digestionWorker.ApplyDigestion(voreProperties, innerContainer);
                if (EnderfreesVoreMod.settings.nutritionGainOption == (int)NutritionGainOptions.PerDigestionTick)
                {
                    Need_Food foodNeed = voreProperties.pred.needs?.TryGetNeed<Need_Food>();

                    if (foodNeed != null)
                    {
                        if (voreProperties.grantsNutrition)
                        {
                            foodNeed.CurLevel += voreProperties.digestionWorker.GetNutritionFromDigestionTick(voreProperties, innerContainer);
                        }

                        foodNeed.CurLevel -= voreProperties.nutritionCost;
                    }
                }

                if (voreProperties.struggle)
                {
                    foreach (Thing thing in innerContainer)
                    {
                        voreProperties.digestionWorker.Struggle(thing, voreProperties);
                    }
                }
            }

            // Sleep if you're not being digested
            if (voreProperties.baseDamage == 0 && !voreProperties.struggle)
            {
                foreach (Thing thing in innerContainer)
                {
                    if (thing is Pawn pawn)
                    {
                        Need_Rest restNeed = pawn.needs?.TryGetNeed<Need_Rest>();

                        if (restNeed != null)
                        {
                            Need_Comfort comfortNeed = pawn.needs?.TryGetNeed<Need_Comfort>();
                            if (comfortNeed != null)
                            {
                                comfortNeed.ComfortUsed(voreProperties.comfort);
                            }

                            restNeed.TickResting(1);
                        }
                    }
                }
            }

            // Move Along
            if (--remainingStageTime <= 0)
            {
                if (voreProperties.trackStage + 1 < voreProperties.digestiveTracks[voreProperties.trackId].track.Count)
                {
                    HediffVore next = (HediffVore)voreProperties.pred.health.AddHediff(InternalDefOf.EVM_Vore, voreProperties.pred.RaceProps.body.GetPartsWithDef(voreProperties.digestiveTracks[voreProperties.trackId].track[voreProperties.trackStage + 1])[0]);
                    next.voreProperties = Utils.GetVorePropertiesFromTags(voreProperties.pred, voreProperties.prey, voreProperties.trackId, voreProperties.trackStage + 1, voreProperties.struggle);
                }
                else
                {
                    FreePreys();
                }
            }

            if (innerContainer.Count <= 0)
            {
                voreProperties.pred.health.RemoveHediff(this);
            }
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);

            FreePreys();
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return this.innerContainer;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
        }

        public IThingHolder ParentHolder
        {
            get
            {
                return this.pawn;
            }
        }

        public override string LabelBase
        { 
            get
            {
                return "Contains: " + voreProperties.prey + (voreProperties.prey.stackCount != 1 ? "x" + voreProperties.prey.stackCount : "");
            }
        }

        public override string LabelInBrackets
        {
            get
            {
                if (voreProperties.prey is Pawn pawn)
                {
                    return pawn.health.summaryHealth.SummaryHealthPercent * 100 + "%" + (pawn.Downed ? " downed" : "");
                }

                return voreProperties.prey.HitPoints / voreProperties.prey.MaxHitPoints * 100 + "%";
            }
        }

        public ThingOwner innerContainer;
        public VoreProperties voreProperties = new VoreProperties();
        public int remainingStageTime;
    }
}
