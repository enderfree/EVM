using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM
{
    internal class GeneExtension: DefModExtension
    {
        public float setMawSize = SwallowWholeLibrary.settings.DefaultMawSize;

        public List<DigestiveTrack> figurativeTracks = new List<DigestiveTrack>();
    }
}
