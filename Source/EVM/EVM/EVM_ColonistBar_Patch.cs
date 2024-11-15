using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace EVM
{
    [HarmonyPatch(typeof(ColonistBar), "CheckRecacheEntries")]
    internal class EVM_ColonistBar_Patch
    {
        /// <summary>
        /// Keep colonists that were eaten in the colonist bar
        /// </summary>
        /// <param name="tmpMaps">Maps</param>
        /// <param name="i">Map i, this method is called in a for iterating tmpMaps</param>
        /// <param name="tmpPawns">"return", to keep a colonist in the bar, it needs to be in this list</param>
        private static void KeepColonistSnacksInColonistBar(List<Map> tmpMaps, int i, List<Pawn> tmpPawns)
        {
            foreach(Pawn pawn in tmpMaps[i].mapPawns.AllPawns)
            {
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                {
                    if (hediff is PreyContainer preyContainer)
                    {
                        foreach (Thing thing in preyContainer.innerContainer)
                        {
                            if (thing is Pawn prey)
                            {
                                if (prey.IsColonist)
                                {
                                    tmpPawns.Add(prey);
                                }
                            }
                        }
                    }
                }
            }
        }

        //static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    List<CodeInstruction> code = new List<CodeInstruction>(instructions);
        //    int stopPoint = -1;

        //    for (int i = 0; i < code.Count; i++)
        //    {
        //        yield return code[i];

        //        if (code[i].Calls(AccessTools.PropertyGetter(typeof(MapPawns), nameof(MapPawns.ColonyMutantsPlayerControlled))))
        //        {
        //            yield return code[++i];
        //            stopPoint = i;
        //            break;
        //        }
        //    }

        //    yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(ColonistBar), "tmpMaps"));
        //    yield return new CodeInstruction(OpCodes.Ldloc_S, 4);
        //    yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(ColonistBar), "tmpPawns"));
        //    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EVM_ColonistBar_Patch), nameof(KeepColonistSnacksInColonistBar)));

        //    for (int i = stopPoint + 1; i < code.Count; i++)
        //    {
        //        yield return code[i];
        //    }
        //}
    }
}
