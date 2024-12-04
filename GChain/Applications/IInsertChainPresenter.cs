using GChain.Domain;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Applications
{
    internal interface IInsertChainPresenter
    {
        void Present(IFunction function, Value? preview);

        void Failed();
    }
}
