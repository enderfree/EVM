using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVM.Digestion;
using EVM.Release;
using RimWorld;
using Verse;

namespace EVM
{
    // this class contain the vore properties and default values needed for vore
    public class SwallowWholeProperties: IExposable
    {
        // static variables
        public static List<SwallowWholeProperties> passer = new List<SwallowWholeProperties>();

        // no default value
        public Pawn pred;
        public Thing prey;

        // constructors
        public SwallowWholeProperties()
        {

        }

        public SwallowWholeProperties(Pawn pred, Thing prey) 
        {
            this.pred = pred;
            this.prey = prey;
        }

        // defined per maw
        public float mawSize = SwallowWholeLibrary.settings.DefaultMawSize; // percent compared to creature size

        // defined per stomach
        public float baseDamage = 10f;
        public float digestionEfficiancy = 1f;
        public DamageDef digestionDamageType = DamageDefOf.AcidBurn;
        public float comfort = 1f;
        public Dictionary<DamageArmorCategoryDef, int> armorValues = new Dictionary<DamageArmorCategoryDef, int>()
        {
            { InternalDefOf.Blunt, 100 },
            { DamageArmorCategoryDefOf.Sharp, 30 }
        };
        public Predicate<Thing> canDigest = (thing => !Utils.IsStoneOrMetal(thing));
        public int deadline = 180000;
        public DigestionWorker digestionWorker = new DigestionWorker_Fatal();
        public bool grantsNutrition = true;
        public float nutritionCost = 0f;
        public bool isTimedStage = true;
        public CustomReleaseWorker customReleaseWorker = null;

        // Defined per body
        public List<DigestiveTrack> digestiveTracks = new List<DigestiveTrack>()
        {
            new DigestiveTrack()
            {
                defName = "Failsafe",
                purpose = "Fatal", 
                track = new List<StomachUnifier>()
                {
                    new StomachUnifier() { stomach = InternalDefOf.Stomach }
                }
            }
        };

        // Defined Per gene  // Move to Utils
        //public List<FigurativeTrack> figurativeTracks = new List<FigurativeTrack>() { 
        //    new FigurativeTrack()
        //    {
        //        purpose = "Fatal",
        //        track = new List<FigurativeStomach>()
        //        {
        //            new FigurativeStomach()
        //            {
        //                actualPart = InternalDefOf.Stomach,
        //                baseDamage = 10f, 
        //                digestionEfficiancy = 1f, 
        //                digestionDamageType = DamageDefOf.AcidBurn, 
        //                comfort = 1f,
        //                armorValues = new Dictionary<DamageArmorCategoryDef, int>()
        //                {
        //                    { InternalDefOf.Blunt, 100 },
        //                    { DamageArmorCategoryDefOf.Sharp, 30 }
        //                }, 
        //                canDigest = (thing => !Utils.IsStoneOrMetal(thing)), 
        //                deadline = 180000, 
        //                digestionWorker = new DigestionWorker_Fatal().GetType(), 
        //                grantsNutrition = true, 
        //                nutritionCost = 0f
        //            }
        //        }
        //    }
        //};

        // here 
        public int trackId = 0; // ID of the track in use for digestiveTracks
        public int trackStage = 0; // ID of the part in track
        public bool struggle = true;

        // Methods
        public virtual bool IsValid(bool throwMessage)
        {
            if (pred == null)
            {
                if (throwMessage)
                {
                    Messages.Message("Vore invalid: pred is null", MessageTypeDefOf.NegativeEvent, false);
                }

                return false;
            }

            if (prey == null)
            {
                if (throwMessage)
                {
                    Messages.Message("Vore invalid: prey is null", MessageTypeDefOf.NegativeEvent, false);
                }

                return false;
            }

            if (trackId >= 0 || trackId < digestiveTracks.Count)
            {
                if (!(trackStage >= 0 || trackStage < digestiveTracks[trackId].track.Count))
                {
                    Log.Error("Vore invalid: unknow stomach");
                }
            }
            else
            {
                Log.Error("Vore invalid: unknown track");
                return false;
            }

            if (!SwallowWholeLibrary.settings.swallowIgnoresSize && prey is Pawn preyPawn)
            {
                if (preyPawn.BodySize > pred.BodySize * mawSize)
                {
                    if (throwMessage)
                    {
                        Messages.Message("Vore invalid: Maw too small", MessageTypeDefOf.NegativeEvent, false);
                    }
                    
                    return false;
                }
            }

            return true;
        }

        public void ExposeData()
        {
            Scribe_References.Look<Pawn>(ref pred, "EVM_SwallowWholeProperties_Pred");
            Scribe_References.Look<Thing>(ref prey, "EVM_SwallowWholeProperties_Prey");

            Scribe_Values.Look<int>(ref trackId, "EVM_SwallowWholeProperties_TrackId");
            Scribe_Values.Look<int>(ref trackStage, "EVM_SwallowWholeProperties_TrackStage");
            Scribe_Values.Look<bool>(ref struggle, "EVM_SwallowWholeProperties_Struggle");

            Scribe_Deep.Look<DigestionWorker>(ref digestionWorker, "EVM_SwallowWholeProperties_DigestionWorker");
            Scribe_Deep.Look<CustomReleaseWorker>(ref customReleaseWorker, "EVM_SwallowWholeProperties_CustomReleaseWorker");

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                SwallowWholeProperties xmlFetcher = Utils.GetSwallowWholePropertiesFromTags(pred, prey, trackId, trackStage, struggle);

                mawSize = xmlFetcher.mawSize;

                digestiveTracks = xmlFetcher.digestiveTracks;

                baseDamage = xmlFetcher.baseDamage;
                digestionEfficiancy = xmlFetcher.digestionEfficiancy;
                digestionDamageType = xmlFetcher.digestionDamageType;
                comfort = xmlFetcher.comfort;
                armorValues = xmlFetcher.armorValues;
                canDigest = xmlFetcher.canDigest;
                deadline = xmlFetcher.deadline;
                grantsNutrition = xmlFetcher.grantsNutrition;
                nutritionCost = xmlFetcher.nutritionCost;
                isTimedStage = xmlFetcher.isTimedStage;

                if (digestionWorker.GetType() != xmlFetcher.digestionWorker.GetType())
                {
                    digestionWorker = xmlFetcher.digestionWorker;
                }

                if (customReleaseWorker != null &&
                    xmlFetcher.customReleaseWorker != null && 
                    customReleaseWorker.GetType() != xmlFetcher.customReleaseWorker.GetType())
                {
                    customReleaseWorker = xmlFetcher.customReleaseWorker;
                }
            }
        }
    }
}
