using GChain.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Infrastructure
{
    internal class TextFileCodeRepository : IGeneratedCodeRepository
    {
        private readonly string filename;

        public TextFileCodeRepository(string filename)
        {
            this.filename = filename;
        }

        public void Save(GeneratedCode code)
        {
            using var sw = new StreamWriter(filename, false, Encoding.UTF8);
            sw.Write(code.Value);
        }
    }
}
