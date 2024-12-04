using GChain.Applications;
using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Adaptors
{
    public class BinaryFunctionController
    {
        private readonly IInsertChainUsecase<GrayMatrix, BinaryMatrix> insertChainUsecase;
        private readonly IAppendChainUsecase<GrayMatrix, BinaryMatrix> appendChainUsecase;

        public BinaryFunctionController(
            IInsertChainUsecase<GrayMatrix, BinaryMatrix> insertChainUsecase,
            IAppendChainUsecase<GrayMatrix, BinaryMatrix> appendChainUsecase)
        {
            this.insertChainUsecase = insertChainUsecase;
            this.appendChainUsecase = appendChainUsecase;
        }

        public void HandleInsert(int threshold)
        {
            insertChainUsecase.Handle(new Binary(threshold));
        }

        public void HandleAppend(int threshold)
        {
            appendChainUsecase.Handle(new Binary(threshold));
        }
    }
}
