using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    public abstract class Value : IDisposable
    {
        public static T MatchType<T>(
            Type type,
            Func<T> bgrMatrixCase,
            Func<T> grayMatrixCase,
            Func<T> binaryMatrixCase,
            Func<T> pointsCase)
        {
            if (type == typeof(BgrMatrix))
            {
                return bgrMatrixCase();
            }

            if (type == typeof(GrayMatrix))
            {
                return grayMatrixCase();
            }

            if (type == typeof(BinaryMatrix))
            {
                return binaryMatrixCase();
            }

            if (type == typeof(Points))
            {
                return pointsCase();
            }

            throw new ArgumentException("invalid type");
        }

        public static T Match<T>(
            Value value,
            Func<BgrMatrix, T> bgrMatrixCase,
            Func<GrayMatrix, T> grayMatrixCase,
            Func<BinaryMatrix, T> binaryMatrixCase,
            Func<Points, T> pointsCase)
        {
            if (value is BgrMatrix bgrMatrix)
            {
                return bgrMatrixCase(bgrMatrix);
            }

            if (value is GrayMatrix grayMatrix)
            {
                return grayMatrixCase(grayMatrix);
            }

            if (value is BinaryMatrix binaryMatrix)
            {
                return binaryMatrixCase(binaryMatrix);
            }

            if (value is Points points)
            {
                return pointsCase(points);
            }

            throw new ArgumentException("invalid type");
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        ~Value()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public sealed class BgrMatrix : Value
    {
        public BgrMatrix(Mat mat)
        {
            if (mat.Type() != MatType.CV_8UC3)
            {
                throw new ArgumentException("CV_8UC3 is required");
            }

            Value = mat;
        }

        public Mat Value { get; }

        private bool isDisposed = false;
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Value.Dispose();
                }

                isDisposed = true;
                base.Dispose(disposing);
            }
        }
    }

    public sealed class GrayMatrix : Value
    {
        public GrayMatrix(Mat mat)
        {
            if (mat.Type() != MatType.CV_8UC1)
            {
                throw new ArgumentException("CV_8UC1 is required");
            }

            Value = mat;
        }

        public Mat Value { get; }

        private bool isDisposed = false;
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Value.Dispose();
                }

                isDisposed = true;
                base.Dispose(disposing);
            }
        }
    }

    public sealed class BinaryMatrix : Value
    {
        public BinaryMatrix(Mat mat)
        {
            if (mat.Type() != MatType.CV_8UC1)
            {
                throw new ArgumentException("CV_8UC1 is required");
            }

            Value = mat;
        }

        public Mat Value { get; }

        private bool isDisposed = false;
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Value.Dispose();
                }

                isDisposed = true;
                base.Dispose(disposing);
            }
        }
    }

    public sealed class Points : Value
    {
        public Points(IEnumerable<Point> points, Mat preview, Mat source)
        {
            Value = points.ToList();
            Preview = preview;
            Source = source;
        }

        public List<Point> Value { get; }
        public Mat Preview { get; }
        public Mat Source { get; }

        private bool isDisposed = false;
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Preview.Dispose();
                    Source.Dispose();
                }

                isDisposed = true;
                base.Dispose(disposing);
            }
        }
    }
}
