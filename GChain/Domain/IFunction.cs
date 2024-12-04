using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GChain.Domain
{
    public interface IFunction
    {
        public static T Match<T>(
            IFunction f,
            Func<Gray, T> grayCase,
            Func<Binary, T> binaryCase,
            Func<FindLargestCountours, T> findLargestContoursCase,
            Func<Approx, T> approxCase,
            Func<Crop, T> cropCase,
            Func<ReadImage, T> readImageCase)
        {
            if (f is Gray gray)
            {
                return grayCase(gray);
            }

            if (f is Binary binary)
            {
                return binaryCase(binary);
            }

            if (f is FindLargestCountours findLargestContours)
            {
                return findLargestContoursCase(findLargestContours);
            }

            if (f is Approx approx)
            {
                return approxCase(approx);
            }

            if (f is Crop crop)
            {
                return cropCase(crop);
            }

            if (f is ReadImage readImage)
            {
                return readImageCase(readImage);
            }

            throw new NotImplementedException();
        }
    }

    public interface IFunction<TIn, TOut> : IFunction
    {
        TOut? Evaluate(TIn input);
        public (GeneratedCode code, string variableName) GenerateCode(string previousVariableName);
    }

    public sealed class Gray : IFunction, IFunction<BgrMatrix, GrayMatrix>
    {
        public GrayMatrix? Evaluate(BgrMatrix input)
        {
            Mat? result = null;
            try
            {
                result = new Mat();
                Cv2.CvtColor(input.Value, result, ColorConversionCodes.BGR2GRAY);
                return new GrayMatrix(result);
            }
            catch
            {
                result?.Dispose();
                return null;
            }
        }

        public (GeneratedCode code, string variableName) GenerateCode(string previousVariableName)
        {
            return (new GeneratedCode($"gray = cv2.cvtColor({previousVariableName}, cv2.COLOR_BGR2GRAY)"), "gray");
        }
    }

    public sealed class Binary : IFunction, IFunction<GrayMatrix, BinaryMatrix>
    {
        private readonly int threshold;

        public Binary(int threshold)
        {
            this.threshold = threshold;
        }

        public BinaryMatrix? Evaluate(GrayMatrix input)
        {
            Mat? result = null;
            try
            {
                result = new Mat();
                Cv2.Threshold(input.Value, result, threshold, 255, ThresholdTypes.Binary);
                return new BinaryMatrix(result);
            }
            catch
            {
                result?.Dispose();
                return null;
            }
        }

        public (GeneratedCode code, string variableName) GenerateCode(string previousVariableName)
        {
            return (new GeneratedCode($"_, binary = cv2.threshold({previousVariableName}, {threshold}, 255, cv2.THRESH_BINARY)"), "binary");
        }
    }

    public sealed class FindLargestCountours : IFunction, IFunction<BinaryMatrix, Points>
    {
        public Points? Evaluate(BinaryMatrix input)
        {
            Mat? preview = null;
            try
            {
                Cv2.FindContours(
                    input.Value,
                    out Point[][] contours,
                    out var _,
                    RetrievalModes.External,
                    ContourApproximationModes.ApproxSimple);

                var largest = contours
                    .Select(c => (area: Cv2.ContourArea(c), contour: c))
                    .OrderBy(p => p.area)
                    .Last()
                    .contour;

                preview = input.Value.Clone();
                Cv2.DrawContours(preview, new[] { largest }, -1, Scalar.Red, thickness: 2);

                return new Points(largest, preview, input.Value.Clone());
            }
            catch
            {
                preview?.Dispose();
                return null;
            }
        }

        public (GeneratedCode code, string variableName) GenerateCode(string previousVariableName)
        {
            return (new GeneratedCode($"contours, _ = cv2.findContours({previousVariableName}, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)\n" +
                $"area_sizes = [cv2.contourArea(c) for c in contours]\n" +
                $"largest_contour, _ = max(zip(contours, area_sizes), key=lambda p: p[1])"), "largest_contour");
        }
    }

    public sealed class Approx : IFunction, IFunction<Points, Points>
    {
        public Points? Evaluate(Points input)
        {

            Mat? preview = null;
            try
            {
                var epsilon = 0.1 * Cv2.ArcLength(input.Value, true);
                var approx = Cv2.ApproxPolyDP(input.Value, epsilon, true);

                preview = input.Source.Clone();
                Cv2.DrawContours(preview, new[] { approx }, -1, Scalar.Red, thickness: 2);

                return new Points(approx, preview, input.Source.Clone());
            }
            catch
            {
                preview?.Dispose();
                return null;
            }
        }

        public (GeneratedCode code, string variableName) GenerateCode(string previousVariableName)
        {
            return (new GeneratedCode($"epsilon = 0.03 * cv2.arcLength({previousVariableName}, True)\n"
                + $"approx = cv2.approxPolyDP({previousVariableName}, epsilon, True)"), "approx");
        }
    }

    public sealed class Crop : IFunction, IFunction<BgrMatrix, BgrMatrix>
    {
        private readonly int x;
        private readonly int y;
        private readonly int width;
        private readonly int height;

        public Crop(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public BgrMatrix? Evaluate(BgrMatrix input)
        {
            try
            {
                return new BgrMatrix(CropImg(input.Value));
            }
            catch
            {
                return null;
            }
        }

        public GrayMatrix? Evaluate(GrayMatrix input)
        {
            try
            {
                return new GrayMatrix(CropImg(input.Value));
            }
            catch
            {
                return null;
            }
        }

        public BinaryMatrix? Evaluate(BinaryMatrix input)
        {
            try
            {
                return new BinaryMatrix(CropImg(input.Value));
            }
            catch
            {
                return null;
            }
        }

        public (GeneratedCode code, string variableName) GenerateCode(string previousVariableName)
        {
            return (new GeneratedCode($"x = {x}\n" +
                $"y = {y}\n" +
                $"width = {width}\n" +
                $"height = {height}\n" +
                $"cropped = {previousVariableName}[y:y+height, x:x+width]"), "cropped");
        }

        private Mat CropImg(Mat img)
        {
            using var roi = new Mat(img, new Rect(x, y, width, height));
            return roi.Clone();
        }
    }

    public sealed class ReadImage : IFunction
    {
        private readonly string filename;

        public ReadImage(string filename)
        {
            this.filename = filename;
        }

        public BgrMatrix? Evaluate()
        {
            Mat? result = null;
            try
            {
                result = Cv2.ImRead(filename);
                return result.Size() != Size.Zero
                    ? new BgrMatrix(result)
                    : null;
            }
            catch
            {
                result?.Dispose();
                return null;
            }
        }

        public (GeneratedCode code, string variableName) GenerateCode()
        {
            return (new GeneratedCode($"img = cv2.imread(\"{filename.Replace("\\", "\\\\")}\")"), "img");
        }
    }
}
