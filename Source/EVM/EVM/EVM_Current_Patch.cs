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
    [HarmonyPatch(typeof(CustomXenotypeDatabase), "ExposeData")]
    internal class EVM_Current_Patch
    {
        [HarmonyPostfix]
        public static void PutCustomXenotypesInXenotypeList(CustomXenotypeDatabase __instance)
        {
            foreach (CustomXenotype customXenotype in __instance.customXenotypes)
            {
                bool found = false;
                foreach (XenotypeUnifier xenotypeUnifier in SwallowWholeLibrary.settings.xenotypes)
                {
                    if (customXenotype.name == xenotypeUnifier.name)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    SwallowWholeLibrary.settings.xenotypes.Add(new XenotypeUnifier() { name = customXenotype.name });
                }
            }
        }
    }
}
