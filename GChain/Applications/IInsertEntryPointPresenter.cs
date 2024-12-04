using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Applications
{
    internal interface IInsertEntryPointPresenter
    {
        void Present(ReadImage entryPoint, Value? preview);

        void Failed();
    }
}
