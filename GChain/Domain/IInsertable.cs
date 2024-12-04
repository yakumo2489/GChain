using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    internal interface IInsertable<TIn>
    {
        Value? Evaluate();
        IChain<TIn> Insert(IChain<TIn> previous);
    }
}
