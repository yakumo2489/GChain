using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Applications
{
    internal class InsertChainInteractor<TIn, TOut> : IInsertChainUsecase<TIn, TOut>
        where TIn : Value
        where TOut : Value
    {
        private readonly IInsertChainPresenter presenter;
        private readonly IInsertableChainStore<TOut> store;

        public InsertChainInteractor(
            IInsertChainPresenter presenter,
            IInsertableChainStore<TOut> store)
        {
            this.presenter = presenter;
            this.store = store;
        }

        public void Handle(IFunction<TIn, TOut> function)
        {
            try
            {
                var chain = new Chain<TIn, TOut>(null, function);

                if (!store.IsInitialized())
                {
                    store.Commit(chain);
                    using var initialPreview = chain.Evaluate();
                    presenter.Present(function, initialPreview);
                    return;
                }

                var headChain = store.GetInsertable();
                if(headChain == null)
                {
                    throw new InvalidOperationException();
                }

                var inserted = headChain.Insert(chain);
                store.Commit(inserted);

                using var preview = inserted.Evaluate();
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
