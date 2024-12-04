using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    internal interface IAppendableChainStore<TIn>
        where TIn : Value
    {
        IChain<TIn>? GetAppendable();
    }
}
