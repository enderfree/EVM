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
    internal class JobDriver_Vore : JobDriver
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
            VoreProperties voreProperties = null;

            foreach (VoreProperties vp in VoreProperties.passer)
            {
                if (vp.pred == this.pawn && vp.prey == OtherPawn)
                {
                    voreProperties = vp;
                    break;
                }
            }

            if (voreProperties != null)
            {
                VoreProperties.passer.Remove(voreProperties);
            }
            else
            {
                voreProperties = Utils.GetVorePropertiesFromTags(this.pawn, OtherPawn);
            }
            
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
            yield return Toils_Vore.Vore(voreProperties);
            yield break;
        }
    }
}
