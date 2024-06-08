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
        /// Gets all the BodyPartExtensions and returns a VoreProperties with them
        /// </summary>
        /// <param name="pred">the pawn from who we get the properties</param>
        /// <param name="prey">not useful, but part of VoreProperties, so might as well assign it now</param>
        /// <param name="trackId">digestive track id in use</param>
        /// <param name="trackStage">id pointing to stomach in track pointed by trackId</param>
        /// <returns></returns>
        public static VoreProperties GetVorePropertiesFromTags(Pawn pred, Thing prey = null, int trackId = 0, int trackStage = 0, bool struggle = true)
        {
            VoreProperties voreProperties = new VoreProperties(pred, prey);
            voreProperties.trackId = trackId;
            voreProperties.trackStage = trackStage;
            voreProperties.struggle = struggle;

            // Maw
            List<BodyPartRecord> jaws = pred.RaceProps.body.GetPartsWithDef(InternalDefOf.Jaw);
            BodyPartExtension bodyPartExtension = jaws[0].def.GetModExtension<BodyPartExtension>();

            if (jaws.Count > 0)
            {
                if (bodyPartExtension != null)
                {
                    if (bodyPartExtension.mawSize != -1)
                    {
                        voreProperties.mawSize = bodyPartExtension.mawSize;
                    }
                }
            }

            // Digestive Tracks
            bodyPartExtension = pred.RaceProps.body.GetModExtension<BodyPartExtension>();
            if (bodyPartExtension != null) 
            { 
                if (bodyPartExtension.digestiveTracks != null)
                {
                    voreProperties.digestiveTracks = bodyPartExtension.digestiveTracks;
                }
            }

            // Stomach
            if (trackId < voreProperties.digestiveTracks.Count)
            {
                if (trackStage < voreProperties.digestiveTracks[trackId].track.Count)
                {
                    bodyPartExtension = voreProperties.digestiveTracks[trackId].track[trackStage].GetModExtension<BodyPartExtension>();

                    if (bodyPartExtension != null)
                    {
                        if (bodyPartExtension.baseDamage != -1)
                        {
                            voreProperties.baseDamage = bodyPartExtension.baseDamage;
                        }

                        if (bodyPartExtension.digestionEfficiancy != -1)
                        {
                            voreProperties.digestionEfficiancy = bodyPartExtension.digestionEfficiancy;
                        }

                        if (bodyPartExtension.digestionDamageType != null)
                        {
                            voreProperties.digestionDamageType = bodyPartExtension.digestionDamageType;
                        }

                        if (bodyPartExtension.comfort != -1)
                        {
                            voreProperties.comfort = bodyPartExtension.comfort;
                        }

                        if (bodyPartExtension.armorValues != null)
                        {
                            voreProperties.armorValues = bodyPartExtension.armorValues;
                        }

                        if (bodyPartExtension.canDigest != null)
                        {
                            voreProperties.canDigest = bodyPartExtension.canDigest;
                        }

                        if (bodyPartExtension.deadline != -1)
                        {
                            voreProperties.deadline = bodyPartExtension.deadline;
                        }

                        if (bodyPartExtension.digestionWorker != null)
                        {
                            if (Activator.CreateInstance(bodyPartExtension.digestionWorker) is DigestionWorker digestionWorker)
                            {
                                voreProperties.digestionWorker = digestionWorker;
                            }
                        }

                        if (bodyPartExtension.grantsNutrition != false)
                        {
                            voreProperties.grantsNutrition = bodyPartExtension.grantsNutrition;
                        }

                        if (bodyPartExtension.nutritionCost != 0)
                        {
                            voreProperties.nutritionCost = bodyPartExtension.nutritionCost;
                        }
                    }
                }
                else
                {
                    Log.Error("[EVM.Utils.GetVorePropertiesFromTags]: trackStage out of bounds");
                }
            }
            else
            {
                Log.Error("[EVM.Utils.GetVorePropertiesFromTags]: trackId out of bounds");
            }

            return voreProperties;
        }

        public static bool Vore(HediffVore hediffVore, Thing thing)
        {
            thing.DeSpawnOrDeselect(DestroyMode.Vanish);

            if (thing.holdingOwner != null)
            {
                thing.holdingOwner.TryTransferToContainer(thing, hediffVore.innerContainer, thing.stackCount, true);
                return true;
            }
            else
            {
                return hediffVore.innerContainer.TryAdd(thing, true);
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
