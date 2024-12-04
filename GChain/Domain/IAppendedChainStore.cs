using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    internal interface IAppendedChainStore<TIn, TOut>
        where TIn : Value
        where TOut : Value
    {
        void Commit(Chain<TIn, TOut> appended);
    }
}
