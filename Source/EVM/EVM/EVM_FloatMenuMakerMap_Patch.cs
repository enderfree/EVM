using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace EVM
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
    internal class EVM_FloatMenuMakerMap_Patch
    {
        [HarmonyPostfix]
        public static void AddVoreOptions(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts) 
        {
            if (EnderfreesVoreMod.settings.voreDebugOptions)
            {
                // For every Pawn in the map
                foreach (LocalTargetInfo localTargetInfo in GenUI.TargetsAt(clickPos, TargetingParameters.ForPawns(), true, null))
                {
                    Pawn food = (Pawn)localTargetInfo.Thing;

                    // Except the selected pawn
                    if (food != pawn)
                    {
                        // Vore
                        VoreProperties voreProperties = Utils.GetVorePropertiesFromTags(pawn, food);

                        for (int i = 0; i < voreProperties.digestiveTracks.Count; ++i)
                        {
                            voreProperties.trackId = i;

                            opts.Add(new FloatMenuOption("Vore " + food.LabelShort + " (" + voreProperties.digestiveTracks[i].purpose + ")", delegate () {
                                VoreProperties.passer.Add(voreProperties);
                                pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.Vore, food));
                            }));
                        }
                    }
                    else
                    {
                        // Regurgitate
                        List<HediffVore> hediffVores = new List<HediffVore>();
                        pawn.health.hediffSet.GetHediffs<HediffVore>(ref hediffVores);

                        foreach (HediffVore hediffVore in hediffVores)
                        {
                            foreach (Thing thing in hediffVore.innerContainer)
                            {
                                opts.Add(new FloatMenuOption("Regurgitate " + thing.LabelShort, delegate () {
                                    if (hediffVore.voreProperties.baseDamage > 0f)
                                    {
                                        pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Vomit));
                                    }

                                    pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.EVM_Regurgitate, thing));
                                }));
                            }
                        }
                    }
                }
            }
        }
    }
}
