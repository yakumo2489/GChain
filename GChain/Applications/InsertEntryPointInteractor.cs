using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Applications
{
    internal class InsertEntryPointInteractor : IInsertEntryPointUsecase
    {
        private readonly IInsertEntryPointPresenter presenter;
        private readonly IInsertableChainStore<BgrMatrix> store;

        public InsertEntryPointInteractor(
            IInsertEntryPointPresenter presenter,
            IInsertableChainStore<BgrMatrix> store)
        {
            this.presenter = presenter;
            this.store = store;
        }

        public void Handle(ReadImage entryPoint)
        {
            try
            {
                var entryPointChain = new EntryPoint(entryPoint);

                if (!store.IsInitialized())
                {
                    store.Commit(entryPointChain);
                    using var initialPreview = entryPointChain.Evaluate();
                    presenter.Present(entryPoint, initialPreview);
                    return;
                }

                var insertable = store.GetInsertable();
                if (insertable == null)
                {
                    throw new InvalidOperationException();
                }

                var inserted = insertable.Insert(entryPointChain);
                store.Commit(inserted);

                using var preview = inserted.Evaluate();
                presenter.Present(entryPoint, preview);
            }
            catch
            {
                presenter.Failed();
                throw;
            }
        }
    }
}
