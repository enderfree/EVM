using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace EVM
{
    internal class XenotypeUnifier: IExposable
    {
        public string defName = "";
        public string label = "";
        public string name = "";
        public float preySize = EnderfreesVoreMod.settings.DefaultMawSize;

        public XenotypeUnifier()
        {
        }

        public void ExposeData()
        {
            Scribe_Values.Look<string>(ref defName, "EVM_XenotypeUnifier_DefName");
            Scribe_Values.Look<string>(ref label, "EVM_XenotypeUnifier_Label");
            Scribe_Values.Look<string>(ref name, "EVM_XenotypeUnifier_Name");
            Scribe_Values.Look<float>(ref preySize, "EVM_XenotypeUnifier_MawSize");
        }

        public override string ToString()
        {
            if (defName != "")
            {
                return label;
            }

            return name;
        }
    }
}
