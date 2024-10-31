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
    public static class Utils
    {
        /// <summary>
        /// Gets all the BodyPartExtensions and returns a SwallowWholeProperties with them
        /// </summary>
        /// <param name="pred">the pawn from who we get the properties</param>
        /// <param name="prey">not useful, but part of SwallowWholeProperties, so might as well assign it now</param>
        /// <param name="trackId">digestive track id in use</param>
        /// <param name="trackStage">id pointing to stomach in track pointed by trackId</param>
        /// <returns></returns>
        public static SwallowWholeProperties GetSwallowWholePropertiesFromTags(Pawn pred, Thing prey = null, int trackId = 0, int trackStage = 0, bool struggle = true)
        {
            SwallowWholeProperties swallowWholeProperties = new SwallowWholeProperties(pred, prey);
            swallowWholeProperties.trackId = trackId;
            swallowWholeProperties.trackStage = trackStage;
            swallowWholeProperties.struggle = struggle;
            //Log.Message("digest");
            // Digestive Tracks
            BodyPartExtension bodyPartExtension = pred.RaceProps.body.GetModExtension<BodyPartExtension>();
            if (bodyPartExtension != null)
            {
                if (bodyPartExtension.digestiveTracks != null)
                {
                    swallowWholeProperties.digestiveTracks = new List<DigestiveTrack>();
                    swallowWholeProperties.digestiveTracks.AddRange(bodyPartExtension.digestiveTracks);
                }
            }
            //Log.Message("maw");
            // Maw
            swallowWholeProperties.mawSize = SwallowWholeLibrary.settings.DefaultMawSize;

            if (pred.RaceProps.Animal)
            {
                IEnumerable<float> matches = from m 
                                             in SwallowWholeLibrary.settings.mawList
                                             where m.defName == pred.kindDef.defName
                                             select m.preySize;

                if (matches.Count() > 0)
                {
                    swallowWholeProperties.mawSize = matches.First();
                }
            } 
            else if (pred.RaceProps.Humanlike)
            {
                if (ModLister.BiotechInstalled)
                {
                    if (pred.genes != null)
                    {
                        bool mawSizeIsHandledByGene = false;
                        foreach (Gene gene in pred.genes.GenesListForReading)
                        {
                            GeneExtension geneExtension = gene.def.GetModExtension<GeneExtension>();

                            if (geneExtension != null)
                            {
                                // load geneExtensions here to avoid to repeat the big if chain
                                if (geneExtension.setMawSize != SwallowWholeLibrary.settings.DefaultMawSize)
                                {
                                    swallowWholeProperties.mawSize = geneExtension.setMawSize;
                                    mawSizeIsHandledByGene = true;
                                }

                                if (geneExtension.figurativeTracks != null)
                                {
                                    foreach (DigestiveTrack track in geneExtension.figurativeTracks)
                                    {
                                        if(!swallowWholeProperties.digestiveTracks.Contains(track))
                                        {
                                            swallowWholeProperties.digestiveTracks.Add(track);
                                        }
                                    }
                                }
                            }
                        }

                        if (!mawSizeIsHandledByGene)
                        {
                            foreach (XenotypeUnifier xenotypeUnifier in SwallowWholeLibrary.settings.xenotypes)
                            {
                                if (pred.genes.Xenotype != null)
                                {
                                    if (xenotypeUnifier.ToString() == pred.genes.Xenotype.defName)
                                    {
                                        swallowWholeProperties.mawSize = xenotypeUnifier.preySize;
                                        break;
                                    }
                                }
                                else if (pred.genes.CustomXenotype != null)
                                {
                                    if (xenotypeUnifier.ToString() == pred.genes.CustomXenotype.name)
                                    {
                                        swallowWholeProperties.mawSize = xenotypeUnifier.preySize;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Log.Message("stomach");
            // Stomach
            if (trackId < swallowWholeProperties.digestiveTracks.Count)
            {
                //Log.Message("track in range");
                if (trackStage < swallowWholeProperties.digestiveTracks[trackId].track.Count)
                {
                    //Log.Message("object in range");
                    StomachUnifier stomach = swallowWholeProperties.digestiveTracks[trackId].track[trackStage];
                    //Log.Message(trackId);
                    if (stomach.stomach != null)
                    {
                        //Log.Message("stomachDef");
                        bodyPartExtension = stomach.stomach.GetModExtension<BodyPartExtension>();

                        if (bodyPartExtension != null)
                        {
                            if (bodyPartExtension.baseDamage != -1)
                            {
                                swallowWholeProperties.baseDamage = bodyPartExtension.baseDamage;
                            }

                            if (bodyPartExtension.digestionEfficiancy != -1)
                            {
                                swallowWholeProperties.digestionEfficiancy = bodyPartExtension.digestionEfficiancy;
                            }

                            if (bodyPartExtension.digestionDamageType != null)
                            {
                                swallowWholeProperties.digestionDamageType = bodyPartExtension.digestionDamageType;
                            }

                            if (bodyPartExtension.comfort != -1)
                            {
                                swallowWholeProperties.comfort = bodyPartExtension.comfort;
                            }

                            if (bodyPartExtension.armorValues != null)
                            {
                                swallowWholeProperties.armorValues = bodyPartExtension.armorValues;
                            }

                            if (bodyPartExtension.canDigest != null)
                            {
                                swallowWholeProperties.canDigest = bodyPartExtension.canDigest;
                            }

                            if (bodyPartExtension.deadline != -1)
                            {
                                swallowWholeProperties.deadline = bodyPartExtension.deadline;
                            }

                            if (bodyPartExtension.digestionWorker != null)
                            {
                                if (Activator.CreateInstance(bodyPartExtension.digestionWorker) is DigestionWorker digestionWorker)
                                {
                                    swallowWholeProperties.digestionWorker = digestionWorker;
                                }
                            }

                            if (bodyPartExtension.grantsNutrition != false)
                            {
                                swallowWholeProperties.grantsNutrition = bodyPartExtension.grantsNutrition;
                            }

                            if (bodyPartExtension.nutritionCost != 0)
                            {
                                swallowWholeProperties.nutritionCost = bodyPartExtension.nutritionCost;
                            }
                        }
                    }
                    else if (stomach.figurativeStomach != null)
                    {
                        //Log.Message("figurative stomach");
                        if (stomach.figurativeStomach.baseDamage != -1)
                        {
                            swallowWholeProperties.baseDamage = stomach.figurativeStomach.baseDamage;
                        }

                        if (stomach.figurativeStomach.digestionEfficiancy != -1)
                        {
                            swallowWholeProperties.digestionEfficiancy = stomach.figurativeStomach.digestionEfficiancy;
                        }

                        if (stomach.figurativeStomach.digestionDamageType != null)
                        {
                            swallowWholeProperties.digestionDamageType = stomach.figurativeStomach.digestionDamageType;
                        }

                        if (stomach.figurativeStomach.comfort != 1)
                        {
                            swallowWholeProperties.comfort = stomach.figurativeStomach.comfort;
                        }

                        if (stomach.figurativeStomach.armorValues != null)
                        {
                            swallowWholeProperties.armorValues = stomach.figurativeStomach.armorValues;
                        }

                        if (stomach.figurativeStomach.canDigest != null)
                        {
                            swallowWholeProperties.canDigest = stomach.figurativeStomach.canDigest;
                        }

                        if (stomach.figurativeStomach.deadline != -1)
                        {
                            swallowWholeProperties.deadline = stomach.figurativeStomach.deadline;
                        }

                        if (stomach.figurativeStomach.digestionWorker != null)
                        {
                            if (Activator.CreateInstance(stomach.figurativeStomach.digestionWorker) is DigestionWorker digestionWorker)
                            {
                                swallowWholeProperties.digestionWorker = digestionWorker;
                            }
                        }

                        if (stomach.figurativeStomach.grantsNutrition != false)
                        {
                            swallowWholeProperties.grantsNutrition = stomach.figurativeStomach.grantsNutrition;
                        }

                        if (stomach.figurativeStomach.nutritionCost != -1)
                        {
                            swallowWholeProperties.nutritionCost = stomach.figurativeStomach.nutritionCost;
                        }
                    }
                }
                else
                {
                    Log.Error("[EVM.Utils.GetSwallowWholePropertiesFromTags]: trackStage out of bounds");
                }
            }
            else
            {
                Log.Error("[EVM.Utils.GetSwallowWholePropertiesFromTags]: trackId out of bounds");
            }
            
            return swallowWholeProperties;
        }

        public static bool SwallowWhole(PreyContainer preyContainer, Thing thing)
        {
            thing.DeSpawnOrDeselect(DestroyMode.Vanish);

            if (thing.holdingOwner != null)
            {
                thing.holdingOwner.TryTransferToContainer(thing, preyContainer.innerContainer, thing.stackCount, true);
                return true;
            }
            else
            {
                return preyContainer.innerContainer.TryAdd(thing, true);
            }
        }

        // Predicates
        public static bool IsStoneOrMetal(Thing thing)
        {
            if (thing.def != null)
            {
                if (thing.def.stuffProps != null)
                {
                    if (thing.def.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic) ||
                        thing.def.stuffProps.categories.Contains(StuffCategoryDefOf.Stony))
                    {
                        return true;
                    }
                }
            }

            if (thing is Pawn pawn)
            {
                if (pawn.RaceProps != null)
                {
                    if (pawn.RaceProps.IsMechanoid)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
