using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;
using Verse.AI;
using UnityEngine;

namespace EVM
{
    [HarmonyPatch(typeof(Toils_Ingest), "FinalizeIngest")]
    internal class EVM_Toils_Ingest_Patch
    {
        //[HarmonyPostfix]
        //public static void VoreFood(Pawn ingester, TargetIndex ingestibleInd, ref Toil __result)
        //{
        //    VoreProperties voreProperties = Utils.GetVorePropertiesFromTags(ingester, ingester.jobs.curJob.GetTarget(ingestibleInd).Thing);
        //    HediffVore hediffVore = (HediffVore)ingester.health.AddHediff(InternalDefOf.EVM_Vore, voreProperties.pred.RaceProps.body.GetPartsWithDef(voreProperties.digestiveTracks[voreProperties.trackId].track[voreProperties.trackStage])[0]);
        //    hediffVore.voreProperties = voreProperties;
        //}
    }
}
