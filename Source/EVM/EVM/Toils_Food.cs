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
    public static class Toils_Food
    {
        public static Toil SwallowWhole(SwallowWholeProperties swallowWholeProperties)
        {
            Toil toil = ToilMaker.MakeToil("SwallowWhole");
            toil.initAction = delegate ()
            {
                if (swallowWholeProperties.pred == null)
                {
                    swallowWholeProperties.pred = toil.actor;
                }

                StomachUnifier stomach = swallowWholeProperties.digestiveTracks[swallowWholeProperties.trackId].track[swallowWholeProperties.trackStage];
                BodyPartDef stomachDef = stomach.stomach ?? stomach.figurativeStomach.actualPart;
                
                if (swallowWholeProperties.IsValid(true))
                {
                    PreyContainer preyContainer = (PreyContainer)swallowWholeProperties.pred.health.AddHediff(InternalDefOf.EVM_PreyContainer, swallowWholeProperties.pred.RaceProps.body.GetPartsWithDef(stomachDef)[0]);
                    preyContainer.swallowWholeProperties = swallowWholeProperties;
                    Utils.SwallowWhole(preyContainer, swallowWholeProperties.prey);
                }
            };

            return toil;
        }

        public static Toil Regurgitate(Thing food)
        {
            Toil toil = ToilMaker.MakeToil("Regurgitate");
            toil.initAction = delegate ()
            {
                GenPlace.TryPlaceThing(food, toil.actor.Position, toil.actor.MapHeld, ThingPlaceMode.Near, null, null, default(Rot4));
            };

            return toil;
        }
    }
}
