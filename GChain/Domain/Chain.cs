using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    internal class Chain<TIn, TOut> : IChain<TOut>, IInsertable<TIn>
        where TIn : Value
        where TOut : Value
    {
        private IChain<TIn>? previous;
        private readonly IFunction<TIn, TOut> function;

        public Chain(IChain<TIn>? previous, IFunction<TIn, TOut> function)
        {
            this.previous = previous;
            this.function = function;
        }

        public IChain<TIn> Insert(IChain<TIn> previous)
        {
            this.previous = previous;
            return previous;
        }

        public TOut? Evaluate()
        {
            if (previous == null)
            {
                return null;
            }

            using var previousResult = previous.Evaluate();
            if (previousResult == null)
            {
                return null;
            }

            return function.Evaluate(previousResult);
        }

        Value? IInsertable<TIn>.Evaluate()
        {
            return Evaluate();
        }

        public (GeneratedCode code, string previousValueName) GenerateCode()
        {
            if(previous == null)
            {
                return function.GenerateCode("value");
            }

            var (previousCode, previousVariableName) = previous.GenerateCode();
            var (code, variableName) = function.GenerateCode(previousVariableName);
            return (code.Insert(previousCode), variableName);
        }
    }
}
