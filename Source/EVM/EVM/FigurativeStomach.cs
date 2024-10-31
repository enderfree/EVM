using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM
{
    public class FigurativeStomach
    {
        // Adding body parts for individuals may have an impact on the whole race, so I made this to emulate stomaches without adding one

        public BodyPartDef actualPart;

        // EVM Stomach Fields
        public float baseDamage = -1;
        public float digestionEfficiancy = -1;
        public DamageDef digestionDamageType;
        public float comfort = -1;
        public Dictionary<DamageArmorCategoryDef, int> armorValues;
        public Predicate<Thing> canDigest; // whitelist
        public int deadline; // the time in tick that food spend in this stomach
        public Type digestionWorker;
        public bool grantsNutrition;
        public float nutritionCost;
    }
}
