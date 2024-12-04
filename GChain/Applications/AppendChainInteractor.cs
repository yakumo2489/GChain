using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Applications
{
    internal class AppendChainInteractor<TIn, TOut> : IAppendChainUsecase<TIn, TOut>
        where TIn : Value
        where TOut : Value
    {
        private readonly IAppendChainPresenter presenter;
        private readonly IAppendableChainStore<TIn> store;
        private readonly IAppendedChainStore<TIn, TOut> appendedChainStore;

        public AppendChainInteractor(
            IAppendChainPresenter presenter,
            IAppendableChainStore<TIn> store,
            IAppendedChainStore<TIn, TOut> appendedChainStore)
        {
            this.presenter = presenter;
            this.store = store;
            this.appendedChainStore = appendedChainStore;
        }

        public void Handle(IFunction<TIn, TOut> function)
        {
            try
            {
                var lastChain = store.GetAppendable();
                if(lastChain == null)
                {
                    throw new InvalidOperationException();
                }

                var chain = new Chain<TIn, TOut>(lastChain, function);
                appendedChainStore.Commit(chain);

                using var preview = chain.Evaluate();
                presenter.Present(function, preview);
            }
            catch
            {
                presenter.Failed();
                return;
            }
        }
    }
}
