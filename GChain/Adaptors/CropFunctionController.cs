using GChain.Applications;
using GChain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Adaptors
{
    public class CropFunctionController
    {
        private readonly IInsertChainUsecase<BgrMatrix, BgrMatrix> insertChainUsecase;
        private readonly IAppendChainUsecase<BgrMatrix, BgrMatrix> appendChainUsecase;

        public CropFunctionController(
            IInsertChainUsecase<BgrMatrix, BgrMatrix> insertChainUsecase,
            IAppendChainUsecase<BgrMatrix, BgrMatrix> appendChainUsecase)
        {
            this.insertChainUsecase = insertChainUsecase;
            this.appendChainUsecase = appendChainUsecase;
        }

        public void HandleInsert(int x, int y, int width, int height)
        {
            if (x < 0 || y < 0 || width <= 0 || height <= 0)
            {
                return;
            }

            insertChainUsecase.Handle(new Crop(x, y, width, height));
        }

        public void HandleAppend(int x, int y, int width, int height)
        {
            if (x < 0 || y < 0 || width <= 0 || height <= 0)
            {
                return;
            }

            appendChainUsecase.Handle(new Crop(x, y, width, height));
        }
    }
}
