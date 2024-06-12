﻿using System;
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

            foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefsListForReading)
            {
                if (pawnKindDef.RaceProps.Animal)
                {
                    bool found = false;
                    foreach (SettingsAnimal animal in SwallowWholeLibrary.settings.mawList)
                    {
                        if (pawnKindDef.defName == animal.defName)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        SettingsAnimal settingsAnimal = new SettingsAnimal(pawnKindDef.label, pawnKindDef.defName);

                        foreach (SettingsAnimal exception in SwallowWholeLibrary.settings.animalMawException)
                        {
                            if (settingsAnimal.defName == exception.defName)
                            {
                                settingsAnimal.preySize = exception.preySize;
                                break;
                            }
                        }

                        SwallowWholeLibrary.settings.mawList.Add(settingsAnimal);
                    }
                }
            }

            foreach (XenotypeDef xenotypeDef in DefDatabase<XenotypeDef>.AllDefsListForReading)
            {
                bool found = false;
                foreach (XenotypeUnifier xenotypeUnifier in SwallowWholeLibrary.settings.xenotypes)
                {
                    if (xenotypeDef.defName == xenotypeUnifier.defName)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    XenotypeUnifier xenotypeUnifier = new XenotypeUnifier()
                    {
                        defName = xenotypeDef.defName,
                        label = xenotypeDef.label
                    };

                    foreach (XenotypeUnifier exception in SwallowWholeLibrary.settings.xenotypeMawException)
                    {
                        if (xenotypeUnifier.ToString() == exception.ToString())
                        {
                            xenotypeUnifier.preySize += exception.preySize;
                            break;
                        }
                    }

                    SwallowWholeLibrary.settings.xenotypes.Add(xenotypeUnifier);
                }
            }
        }
    }
}
