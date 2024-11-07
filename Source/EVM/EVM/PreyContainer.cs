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
    public class PreyContainer: HediffWithComps, IThingHolder, IExposable
    {
        public override bool TryMergeWith(Hediff other)
        {
            return false;
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
            remainingStageTime = swallowWholeProperties.deadline;

            if (swallowWholeProperties.pred == null)
            {
                swallowWholeProperties.pred = this.pawn;
            }

            if (SwallowWholeLibrary.settings.nutritionGainOption == (int)NutritionGainOptions.OnEating)
            {
                swallowWholeProperties.digestionWorker.GetNutritionFromDigestion(swallowWholeProperties, innerContainer);
            }
        }

        public void FreePreys()
        {
            foreach (Thing thing in innerContainer)
            {
                GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.MapHeld, ThingPlaceMode.Near, null, null, default(Rot4));
            }
        }

        public override void PostRemoved()
        {
            FreePreys();

            base.PostRemoved();
        }

        public override void Tick()
        {
            base.Tick();
            
            if (Find.TickManager.TicksGame % 530 == 0)
            {
                // digestion
                swallowWholeProperties.digestionWorker.ApplyDigestion(swallowWholeProperties, innerContainer);
                if (SwallowWholeLibrary.settings.nutritionGainOption == (int)NutritionGainOptions.PerDigestionTick)
                {
                    Need_Food foodNeed = swallowWholeProperties.pred.needs?.TryGetNeed<Need_Food>();

                    if (foodNeed != null)
                    {
                        if (swallowWholeProperties.grantsNutrition)
                        {
                            foodNeed.CurLevel += swallowWholeProperties.digestionWorker.GetNutritionFromDigestionTick(swallowWholeProperties, innerContainer);
                        }

                        foodNeed.CurLevel -= swallowWholeProperties.nutritionCost;
                    }
                }

                if (swallowWholeProperties.struggle)
                {
                    foreach (Thing thing in innerContainer)
                    {
                        swallowWholeProperties.digestionWorker.Struggle(thing, swallowWholeProperties);
                    }
                }
            }

            // Sleep if you're not being digested
            if (swallowWholeProperties.baseDamage == 0 && !swallowWholeProperties.struggle)
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
                                comfortNeed.ComfortUsed(swallowWholeProperties.comfort);
                            }

                            restNeed.TickResting(1);
                        }
                    }
                }
            }

            // Move Along
            if (--remainingStageTime <= 0)
            {
                if (swallowWholeProperties.trackStage + 1 < swallowWholeProperties.digestiveTracks[swallowWholeProperties.trackId].track.Count)
                {
                    StomachUnifier stomach = swallowWholeProperties.digestiveTracks[swallowWholeProperties.trackId].track[swallowWholeProperties.trackStage + 1];
                    BodyPartDef stomachDef = stomach.stomach ?? stomach.figurativeStomach.actualPart;

                    PreyContainer next = (PreyContainer)swallowWholeProperties.pred.health.AddHediff(InternalDefOf.EVM_PreyContainer, swallowWholeProperties.pred.RaceProps.body.GetPartsWithDef(stomachDef)[0]);
                    next.swallowWholeProperties = Utils.GetSwallowWholePropertiesFromTags(swallowWholeProperties.pred, swallowWholeProperties.prey, swallowWholeProperties.trackId, swallowWholeProperties.trackStage + 1, swallowWholeProperties.struggle);
                }
                else
                {
                    FreePreys();
                }
            }

            if (innerContainer.Count <= 0)
            {
                swallowWholeProperties.pred.health.RemoveHediff(this);
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

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Thing thing in innerContainer)
            {
                if (thing is Pawn pawn)
                {
                    yield return ContainingSelectionUtility.CreateSelectStorageGizmo(
                        "Select Prey", // name
                        pawn.Name.ToStringShort, // desc
                        thing, // thing to select
                        thing // icon
                    );
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref remainingStageTime, "EVM_PreyContainer_RemainingStageTime");
            Scribe_Deep.Look<SwallowWholeProperties>(ref swallowWholeProperties, "EVM_PreyContainer_SwallowWholeProperties");
            Scribe_Deep.Look<ThingOwner>(ref innerContainer, "EVM_PreyContainer_InnerContainer", new object[] { this });
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
                return "Contains: " + swallowWholeProperties.prey + (swallowWholeProperties.prey.stackCount != 1 ? "x" + swallowWholeProperties.prey.stackCount : "");
            }
        }

        public override string LabelInBrackets
        {
            get
            {
                if (swallowWholeProperties.prey is Pawn pawn)
                {
                    return pawn.health.summaryHealth.SummaryHealthPercent * 100 + "%" + (pawn.Downed ? " downed" : "");
                }

                return swallowWholeProperties.prey.HitPoints / swallowWholeProperties.prey.MaxHitPoints * 100 + "%";
            }
        }

        public override string Description
        {
            get
            {
                return swallowWholeProperties.prey.DescriptionFlavor;
            }
        }

        public ThingOwner innerContainer;
        public SwallowWholeProperties swallowWholeProperties = new SwallowWholeProperties();
        public int remainingStageTime;
    }
}
