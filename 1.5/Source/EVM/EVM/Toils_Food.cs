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

                Utils.SwallowWhole(swallowWholeProperties);
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
