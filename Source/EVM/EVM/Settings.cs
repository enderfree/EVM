using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM
{
    internal class Settings: ModSettings
    {
        // Mod Options
        public int nutritionGainOption = (int)NutritionGainOptions.OnEating;
        public bool swallowIgnoresSize = false;
        public bool voreDebugOptions = false;
        private float defaultMawSize = 0.8f;
        public bool predatorsSwallow = true;

        // Animal Maw
        public List<SettingsAnimal> mawList = new List<SettingsAnimal>();

        // Xenotype Maw
        public List<XenotypeUnifier> xenotypes = new List<XenotypeUnifier>();

        // Alien Maw

        public override void ExposeData()
        {
            // Mod Options
            Scribe_Values.Look<int>(ref nutritionGainOption, "EVM_NutritionGainOption");
            Scribe_Values.Look<bool>(ref swallowIgnoresSize, "EVM_SwallowIgnoresSize");
            Scribe_Values.Look<bool>(ref voreDebugOptions, "EVM_VoreDebugOptions");
            Scribe_Values.Look<float>(ref defaultMawSize, "Evm_DefaultMawSize");
            Scribe_Values.Look<bool>(ref predatorsSwallow, "EVM_PredatorsSwallow");

            // Animal Maw
            Scribe_Collections.Look(ref mawList, "EVM_MawList", LookMode.Deep);

            // Xenotype Maw
            Scribe_Collections.Look(ref xenotypes, "EVM_Xenotypes", LookMode.Deep);

            // Alien Maw
        }

        // Wont be saved
        public SettingTabs tab = SettingTabs.ModOptions;
        public List<SettingsAnimal> animalMawException = new List<SettingsAnimal>() { 
            new SettingsAnimal() { defName = "Cobra", preySize = 2f }
        };
        public List<XenotypeUnifier> xenotypeMawException = new List<XenotypeUnifier>();

        // Getters and Setters
        public float DefaultMawSize
        {
            get 
            { 
                return defaultMawSize; 
            }

            set
            {
                if (value != defaultMawSize)
                {
                    foreach (SettingsAnimal settingsAnimal in mawList)
                    {
                        if (settingsAnimal.preySize == defaultMawSize)
                        {
                            settingsAnimal.preySize = value;
                        }
                    }

                    foreach (XenotypeUnifier xenotype in xenotypes)
                    {
                        if (xenotype.preySize == defaultMawSize)
                        {
                            xenotype.preySize = value;
                        }
                    }

                    defaultMawSize = value;
                }
            }
        }
    }
}
