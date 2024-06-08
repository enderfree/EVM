using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM
{
    [DefOf]
    public static class InternalDefOf
    {
        public static HediffDef EVM_Vore;

        public static JobDef Vore;
        public static JobDef EVM_Regurgitate;

        // Core
        public static DamageArmorCategoryDef Blunt;

        public static BodyPartDef Stomach;
        public static BodyPartDef Jaw;

        public static PawnCapacityDef Metabolism;
    }
}
