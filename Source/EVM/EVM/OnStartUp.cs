using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RimWorld;
using Verse;
using HarmonyLib;

namespace EVM
{
    [StaticConstructorOnStartup]
    public static class OnStartUp
    {
        static OnStartUp() 
        {
            Harmony harmony = new Harmony("com.evm");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
