using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EVM.Release
{
    public abstract class CustomReleaseWorker: IExposable
    {
        public abstract bool ShouldRelease(SwallowWholeProperties swallowWholeProperties);

        public void ExposeData()
        {

        }
    }
}
