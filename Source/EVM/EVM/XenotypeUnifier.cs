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
        public string defName;
        public string label;
        public string name;
        public float preySize;

        public XenotypeUnifier(string defName, string label, string name, float preySize) 
        { 
            this.defName = defName;
            this.label = label;
            this.name = name;
            this.preySize = preySize;
        }

        public XenotypeUnifier(string defName, string label, float preySize)
        {
            this.defName = defName;
            this.label = label;
            this.preySize = preySize;
        }

        public XenotypeUnifier(string name, float preySize)
        {
            this.name = name;
            this.preySize = preySize;
        }

        public XenotypeUnifier(string defName, string label)
        {
            this.defName = defName;
            this.label = label;
            this.preySize = SwallowWholeLibrary.settings.DefaultMawSize;
        }

        public XenotypeUnifier(string name)
        {
            this.name = name;
            this.preySize = SwallowWholeLibrary.settings.DefaultMawSize;
        }

        // Mandatory
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
