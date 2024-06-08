using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM
{
    public class DigestiveTrack
    {
        public string purpose;
        public List<BodyPartDef> track; // do not put the same stomach def twice as the same will be called instead of a second one, which can cause an infinite look.
    }
}
