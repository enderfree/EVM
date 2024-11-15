using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Abilities
{
    public class ConsumeBase: CompAbilityEffect
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
                this.InterceptionLevel(pawn);
            }
        }

        public virtual void InterceptionLevel(Pawn prey)
        {
            this.Consume(prey, "EVM_XenoDefaultTract", true);
        }

        public virtual void Consume(Pawn prey, string trackDefName, bool struggle)
        {
            if (!struggle || !prey.RaceProps.Humanlike || Utils.CanSuccessfullyConsumeUnwilling(this.parent.pawn, prey))
            {
                SwallowWholeProperties swallowWholeProperties = Utils.GetSwallowWholePropertiesFromTags(this.parent.pawn, prey);
                swallowWholeProperties.trackId = swallowWholeProperties.digestiveTracks.FindIndex(t => t.defName == trackDefName);
                swallowWholeProperties = Utils.GetSwallowWholePropertiesFromTags(this.parent.pawn, prey, swallowWholeProperties.trackId, 0, struggle);
                
                Utils.SwallowWhole(swallowWholeProperties);
            }
            else
            {
                Messages.Message(prey.Name.ToStringShort + " managed to weasel their way out of " + this.parent.pawn.Name.ToStringShort + "'s maw!", MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
