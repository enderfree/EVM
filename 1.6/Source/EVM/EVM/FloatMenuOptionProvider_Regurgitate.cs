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
    public class FloatMenuOptionProvider_Regurgitate: FloatMenuOptionProvider
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

        protected override bool CanSelfTarget
        {
            get 
            {
                return true;
            }
        }

        public override bool SelectedPawnValid(Pawn pawn, FloatMenuContext context)
        {
            return SwallowWholeLibrary.settings.debugOptions && base.SelectedPawnValid(pawn, context) && pawn.health.hediffSet.HasHediff<PreyContainer>();
        }

        public override bool TargetPawnValid(Pawn pawn, FloatMenuContext context)
        {
            return true; // I technically already verified by this point
        }

        public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
        {
            List<PreyContainer> preyContainers = new List<PreyContainer>();
            clickedPawn.health.hediffSet.GetHediffs<PreyContainer>(ref preyContainers);

            foreach (PreyContainer preyContainer in preyContainers)
            {
                foreach (Thing thing in preyContainer.innerContainer)
                {
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Regurgitate " + thing.LabelShort, delegate ()
                    {
                        if (preyContainer.swallowWholeProperties.baseDamage > 0f)
                        {
                            clickedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Vomit));
                        }

                        clickedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.EVM_Regurgitate, thing));
                    }), clickedPawn, clickedPawn);
                }
            }

            yield break;
        }
    }
}
