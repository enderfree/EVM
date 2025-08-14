using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace EVM
{
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
    internal class EVM_PawnGenerator_Patch
    {
        [HarmonyPostfix]
        static void AddDigestionWorkerGeneLogic(PawnGenerationRequest request, ref Pawn __result)
        {
            if (__result.genes != null)
            {
                foreach (Gene gene in __result.genes.GenesListForReading)
                {
                    if (gene.def.displayCategory == InternalDefOf.EVM_DigestionWorker)
                    {
                        
                    }
                }
            }
        }
    }
}
