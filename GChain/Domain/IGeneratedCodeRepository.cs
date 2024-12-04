using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    internal interface IGeneratedCodeRepository
    {
        void Save(GeneratedCode code);
    }
}
