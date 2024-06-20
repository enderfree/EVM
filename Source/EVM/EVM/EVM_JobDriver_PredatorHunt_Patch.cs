using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace EVM
{
    [HarmonyPatch(typeof(JobDriver_PredatorHunt), "MakeNewToils")]
    internal class EVM_JobDriver_PredatorHunt_Patch
    {
        [HarmonyPrefix]
        static bool DecideIfPredVoresOrNot(JobDriver_PredatorHunt __instance, out bool __state)
        {
            __state = false;
            
            if (SwallowWholeLibrary.settings.predatorsSwallow && Utils.GetSwallowWholePropertiesFromTags(__instance.pawn, __instance.Prey).IsValid(false))
            {
                __state = true;
            }
            
            return true;
        }

        static IEnumerable<Toil> SwallowWholeIfYouShould(IEnumerable<Toil> toils, JobDriver_PredatorHunt __instance, bool __state, MethodBase __originalMethod, bool ___firstHit, bool ___notifiedPlayerAttacked)
        {
            //Log.Message("reach post");
            if (__state)
            {
                //Log.Message("Swallow Whole");
                yield return Toils_General.DoAtomic(delegate
                {
                    __instance.pawn.MapHeld.attackTargetsCache.UpdateTarget(__instance.pawn);
                });

                Pawn prey = __instance.Prey;
                Action sendMessageIfNeeded = delegate ()
                {
                    bool surpriseAttack = ___firstHit && !prey.IsColonist;
                    if (__instance.pawn.meleeVerbs.TryMeleeAttack(prey, __instance.job.verbToUse, surpriseAttack))
                    {
                        if (!___notifiedPlayerAttacked && PawnUtility.ShouldSendNotificationAbout(prey))
                        {
                            ___notifiedPlayerAttacked = true;
                            Messages.Message("MessageAttackedByPredator".Translate(prey.LabelShort, __instance.pawn.LabelIndefinite(), prey.Named("PREY"), __instance.pawn.Named("PREDATOR")).CapitalizeFirst(), prey, MessageTypeDefOf.ThreatSmall, true);
                        }
                        __instance.pawn.MapHeld.attackTargetsCache.UpdateTarget(__instance.pawn);
                        ___firstHit = false;
                    }
                };
                sendMessageIfNeeded();

                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
                yield return Toils_Food.SwallowWhole(Utils.GetSwallowWholePropertiesFromTags(__instance.pawn, __instance.Prey));
                yield break;
            }
            else
            {
                //Log.Message("Hunt");
                //Log.Message(toils.Count().ToString());
                foreach (Toil toil in toils)
                {
                    yield return toil;
                }
                yield break;
            }
        }
    }
}
