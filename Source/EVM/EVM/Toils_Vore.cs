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
    public static class Toils_Vore
    {
        public static Toil Vore(VoreProperties voreProperties)
        {
            Toil toil = ToilMaker.MakeToil("Vore");
            toil.initAction = delegate ()
            {
                if (voreProperties.pred == null)
                {
                    voreProperties.pred = toil.actor;
                }
                
                if (voreProperties.IsValid(true))
                {
                    HediffVore hediffVore = (HediffVore)voreProperties.pred.health.AddHediff(InternalDefOf.EVM_Vore, voreProperties.pred.RaceProps.body.GetPartsWithDef(voreProperties.digestiveTracks[voreProperties.trackId].track[voreProperties.trackStage])[0]);
                    hediffVore.voreProperties = voreProperties;
                    Utils.Vore(hediffVore, voreProperties.prey);
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
