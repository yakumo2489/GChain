using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    internal interface IChain
    {
        (GeneratedCode code, string previousValueName) GenerateCode();
    }
    internal interface IChain<TOut> : IChain
    {
        TOut? Evaluate();
    }
}
