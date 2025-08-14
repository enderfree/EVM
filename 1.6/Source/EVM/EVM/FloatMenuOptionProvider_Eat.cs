using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;

namespace EVM
{
    public class FloatMenuOptionProvider_Eat: FloatMenuOptionProvider
    {
        protected override bool Multiselect
        { 
            get
            {
                return false;
            }
        }

        protected override bool Drafted
        {
            get
            {
                return true;
            }
        }

        protected override bool Undrafted
        {
            get
            {
                return true;
            }
        }

        protected override bool MechanoidCanDo
        {
            get
            {
                return true;
            }
        }

        public override bool SelectedPawnValid(Pawn pawn, FloatMenuContext context)
        {
            SwallowWholeProperties swallowWholeProperties = Utils.GetSwallowWholePropertiesFromTags(pawn);

            return SwallowWholeLibrary.settings.debugOptions && base.SelectedPawnValid(pawn, context) && swallowWholeProperties.digestiveTracks.Count > 0;
        }

        public override bool TargetPawnValid(Pawn pawn, FloatMenuContext context)
        {
            SwallowWholeProperties swallowWholeProperties = Utils.GetSwallowWholePropertiesFromTags(context.FirstSelectedPawn, pawn);

            return base.TargetPawnValid(pawn, context) && context.FirstSelectedPawn.CanReach(pawn, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && swallowWholeProperties.IsValid(false);
        }

        public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
        {
            SwallowWholeProperties swallowWholeProperties = Utils.GetSwallowWholePropertiesFromTags(context.FirstSelectedPawn, clickedPawn);

            for (int i = 0; i < swallowWholeProperties.digestiveTracks.Count; ++i)
            {
                SwallowWholeProperties trackedSwallowWholeProperties = Utils.GetSwallowWholePropertiesFromTags(context.FirstSelectedPawn, clickedPawn, i);

                yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Eat " + clickedPawn.LabelShort + " (" + trackedSwallowWholeProperties.digestiveTracks[i].purpose + ")", delegate () {
                    SwallowWholeProperties.passer.Add(trackedSwallowWholeProperties);
                    context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.EVM_Eat, clickedPawn));
                }), context.FirstSelectedPawn, clickedPawn);
            }

            yield break;
        }
    }
}
