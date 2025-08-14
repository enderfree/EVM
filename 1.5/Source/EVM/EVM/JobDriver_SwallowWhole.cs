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
    internal class JobDriver_SwallowWhole : JobDriver
    {
        private Pawn OtherPawn
        {
            get
            {
                return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            SwallowWholeProperties swallowWholeProperties = null;

            foreach (SwallowWholeProperties vp in SwallowWholeProperties.passer)
            {
                if (vp.pred == this.pawn && vp.prey == OtherPawn)
                {
                    swallowWholeProperties = vp;
                    break;
                }
            }

            if (swallowWholeProperties != null)
            {
                SwallowWholeProperties.passer.Remove(swallowWholeProperties);
            }
            else
            {
                swallowWholeProperties = Utils.GetSwallowWholePropertiesFromTags(this.pawn, OtherPawn);
            }
            
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
            yield return Toils_Food.SwallowWhole(swallowWholeProperties);
            yield break;
        }
    }
}
