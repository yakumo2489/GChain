using GChain.Applications;
using GChain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Adaptors
{
    internal class GenerateCodeController
    {
        private readonly IGenerateCodeUsecase usecase;

        public GenerateCodeController(IGenerateCodeUsecase usecase)
        {
            this.usecase = usecase;
        }

        public void Handle(string filename)
        {
            usecase.Handle(new TextFileCodeRepository(filename));
        }
    }
}
