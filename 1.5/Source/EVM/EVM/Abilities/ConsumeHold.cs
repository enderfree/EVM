using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Abilities
{
    public class ConsumeHold: ConsumeBase
    {
        public override void InterceptionLevel(Pawn pawn)
        {
            this.Consume(pawn, "EVM_XenoDefaultJailTract", true);
        }
    }
}
