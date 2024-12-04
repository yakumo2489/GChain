using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    internal class EntryPoint : IChain<BgrMatrix>
    {
        private readonly ReadImage readImage;

        public EntryPoint(ReadImage readImage)
        {
            this.readImage = readImage;
        }

        public BgrMatrix? Evaluate()
        {
            return readImage.Evaluate();
        }

        public (GeneratedCode code, string previousValueName) GenerateCode()
        {
            return readImage.GenerateCode();
        }
    }
}
