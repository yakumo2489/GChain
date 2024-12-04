using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Applications
{
    public interface IAppendChainUsecase<TIn, TOut> 
        where TIn : Value
        where TOut : Value
    {
        void Handle(IFunction<TIn, TOut> function);
    }
}
