using GChain.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GChain.Adaptors
{
    internal class GenerateCodePresenter : IGenerateCodePresenter
    {
        public void Failed()
        {
            MessageBox.Show("コード生成に失敗しました");
        }

        public void Present()
        {
            MessageBox.Show("コード生成に成功しました");
        }
    }
}
