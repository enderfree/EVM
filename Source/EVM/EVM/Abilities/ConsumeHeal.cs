using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Abilities
{
    public class ConsumeHeal: ConsumeBase
    {
        public override void InterceptionLevel(Pawn pawn)
        {
            Log.Message("reach the override");
            this.Consume(pawn, "EVM_XenoDefaultHealingTract", false);
        }
    }
}
