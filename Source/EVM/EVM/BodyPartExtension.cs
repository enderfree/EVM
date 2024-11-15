using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using EVM.Digestion;

namespace EVM
{
    internal class BodyPartExtension: DefModExtension
    {
        // Jaw
        //public float mawSize = -1;

        // Stomach
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
        public bool isTimedStage;
        public Type customReleaseWorker;

        // Body
        public List<DigestiveTrack> digestiveTracks; 
    }
}
