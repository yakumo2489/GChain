using GChain.Applications;
using GChain.Domain;
using GChain.ViewModels;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GChain.Adaptors
{
    internal class FunctionPresenter : IInsertEntryPointPresenter, IInsertChainPresenter, IAppendChainPresenter
    {
        public MainWindowViewModel? ViewModel { get; set; }

        public void Failed()
        {
            MessageBox.Show("関数の追加に失敗しました");
        }

        void IInsertEntryPointPresenter.Present(ReadImage entryPoint, Value? preview)
        {
            if(ViewModel == null)
            {
                return;
            }

            var vm = CreateViewModel(entryPoint, preview);
            ViewModel.InsertConnectedFunction(vm);
        }

        void IInsertChainPresenter.Present(IFunction function, Value? preview)
        {
            if (ViewModel == null)
            {
                return;
            }

            var vm = CreateViewModel(function, preview);
            ViewModel.InsertConnectedFunction(vm);
        }

        void IAppendChainPresenter.Present(IFunction function, Value? preview)
        {
            if (ViewModel == null)
            {
                return;
            }

            var vm = CreateViewModel(function, preview);
            ViewModel.AppendConnectedFunction(vm);
        }

        private static ConnectedFunctionViewModel CreateViewModel(IFunction entryPoint, Value? result)
        {
            var (functionName, previousType, nextType) = IFunction.Match(
                entryPoint,
                f => ("グレースケール化", typeof(BgrMatrix), typeof(GrayMatrix)),
                f => ("二値化", typeof(GrayMatrix), typeof(BinaryMatrix)),
                f => ("最も大きい輪郭の検出", typeof(BinaryMatrix), typeof(Points)),
                f => ("輪郭の直線近似", typeof(Points), typeof(Points)),
                f => ("切り抜き", typeof(BgrMatrix), typeof(BgrMatrix)),
                f => ("画像の読み込み", (Type?)null, typeof(BgrMatrix)));

            var viewingPreview = result == null
                ? CreateEmptyPreview()
                : Value.Match(
                result,
                v => MatToImageSource(v.Value),
                v => MatToImageSource(v.Value),
                v => MatToImageSource(v.Value),
                v => MatToImageSource(v.Preview));

            var vm = new ConnectedFunctionViewModel(
                functionName,
                previousType?.ToString(),
                nextType?.ToString(),
                viewingPreview,
                ValueTypeToBrush(previousType),
                ValueTypeToBrush(nextType));

            return vm;
        }

        private static Brush ValueTypeToBrush(Type? valueType)
        {
            if (valueType == null)
            {
                return Brushes.Transparent;
            }

            return Value.MatchType(
                valueType,
                () => Brushes.Red,
                () => Brushes.Green,
                () => Brushes.Blue,
                () => Brushes.Orange);
        }

        private static ImageSource CreateEmptyPreview()
        {
            using var mat = new Mat(100, 100, MatType.CV_8UC3, Scalar.White);
            Cv2.Line(mat, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(100, 100), Scalar.Red, thickness: 5);
            Cv2.Line(mat, new OpenCvSharp.Point(0, 100), new OpenCvSharp.Point(100, 0), Scalar.Red, thickness: 5);

            return MatToImageSource(mat);
        }

        private static ImageSource MatToImageSource(Mat mat)
        {
            var img = mat.ToBitmapSource();
            if (!img.IsFrozen)
            {
                img.Freeze();
            }

            return img;
        }
    }
}
