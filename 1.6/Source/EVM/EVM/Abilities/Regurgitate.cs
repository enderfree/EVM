using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Abilities
{
    public class Regurgitate: CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn pawn = target.Pawn;
            if (pawn == null)
            {
                pawn = dest.Pawn;
            }

            if (pawn != null)
            {
                this.StomachEmptier(pawn);
            }
        }

        public void StomachEmptier(Pawn pawn) 
        {
            List<PreyContainer> preyContainers = new List<PreyContainer>();
            pawn.health.hediffSet.GetHediffs<PreyContainer>(ref preyContainers);

            foreach (PreyContainer preyContainer in preyContainers)
            {
                preyContainer.FreePreys();
            }
        }
    }
}
