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
    [HarmonyPatch(typeof(GameDataSaveLoader), "SaveXenotype")]
    internal class EVM_GameDataSaveLoader_Patch
    {
        [HarmonyPostfix]
        public static void AddToXenotypeList(CustomXenotype xenotype, string absFilePath)
        {
            SwallowWholeLibrary.settings.xenotypes.Add(new XenotypeUnifier(xenotype.name));
        }
    }
}
