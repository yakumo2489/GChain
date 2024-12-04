using GChain.Applications;
using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Adaptors
{
    public class ReadImageFunctionController
    {
        private readonly IInsertEntryPointUsecase insertEntryPointUsecase;

        public ReadImageFunctionController(
            IInsertEntryPointUsecase insertEntryPointUsecase)
        {
            this.insertEntryPointUsecase = insertEntryPointUsecase;
        }

        public void HandleInsert(string? filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            insertEntryPointUsecase.Handle(new ReadImage(filename));
        }
    }
}
