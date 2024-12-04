using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Applications
{
    public interface IInsertEntryPointUsecase
    {
        void Handle(ReadImage entryPoint);
    }
}
