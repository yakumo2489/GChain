using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Applications
{
    internal class GenerateCodeInteractor : IGenerateCodeUsecase
    {
        private readonly IGenerateCodePresenter presenter;
        private readonly IChainStore store;

        public GenerateCodeInteractor(
            IGenerateCodePresenter presenter,
            IChainStore store)
        {
            this.presenter = presenter;
            this.store = store;
        }

        public void Handle(IGeneratedCodeRepository repository)
        {
            try
            {
                var tail = store.PullTailChain();
                if (tail == null)
                {
                    throw new InvalidOperationException();
                }

                var (code, _) = tail.GenerateCode();

                repository.Save(code);
                presenter.Present();
            }
            catch
            {
                presenter.Failed();
                return;
            }
        }
    }
}
