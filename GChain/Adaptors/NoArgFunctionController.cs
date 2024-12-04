using GChain.Applications;
using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Adaptors
{
    public class NoArgFunctionController<TIn, TOut>
        where TIn : Value
        where TOut : Value
    {
        private readonly Func<IFunction<TIn, TOut>> createFunction;
        private readonly IInsertChainUsecase<TIn, TOut> insertChainUsecase;
        private readonly IAppendChainUsecase<TIn, TOut> appendChainUsecase;

        public NoArgFunctionController(
            Func<IFunction<TIn, TOut>> createFunction,
            IInsertChainUsecase<TIn, TOut> insertChainUsecase,
            IAppendChainUsecase<TIn, TOut> appendChainUsecase)
        {
            this.createFunction = createFunction;
            this.insertChainUsecase = insertChainUsecase;
            this.appendChainUsecase = appendChainUsecase;
        }

        public void HandleInsert()
        {
            insertChainUsecase.Handle(createFunction());
        }

        public void HandleAppend()
        {
            appendChainUsecase.Handle(createFunction());
        }
    }
}
