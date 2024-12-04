using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    internal interface IInsertableChainStore<TOut> where TOut : Value
    {
        bool IsInitialized();
        IInsertable<TOut>? GetInsertable();
        void Commit(IChain<TOut> inserted);
    }
}
