using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    public class GeneratedCode
    {
        public GeneratedCode(string code)
        {
            Value = code;
        }

        public GeneratedCode Insert(GeneratedCode code)
        {
            return new GeneratedCode(code.Value + "\n" + this.Value);
        }

        public string Value { get; }
    }
}
