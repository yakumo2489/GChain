using GChain.Adaptors;
using GChain.Domain;
using Livet;
using Livet.Behaviors.Messaging.IO;
using Livet.Commands;
using Livet.Messaging.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GChain.ViewModels
{
    public abstract class SelectableFunctionViewModel : ViewModel
    {
        public SelectableFunctionViewModel()
        {
            PreviousType = GetPreviousType?.ToString();
            NextType = GetNextType.ToString();
        }

        protected abstract Type? GetPreviousType { get; }
        protected abstract Type GetNextType { get; }

        public abstract string? FunctionName { get; }
        public string? PreviousType { get; }
        public string? NextType { get; }

        public abstract ViewModelCommand? InsertCommand { get; }
        public abstract ViewModelCommand? AppendCommand { get; }
    }

    public class GrayFunctionViewModel : SelectableFunctionViewModel
    {
        public GrayFunctionViewModel(NoArgFunctionController<BgrMatrix, GrayMatrix> controller)
        {
            this.controller = controller;
        }

        public override string? FunctionName => "グレースケール化";
        protected override Type GetPreviousType => typeof(BgrMatrix);
        protected override Type GetNextType => typeof(GrayMatrix);

        private readonly NoArgFunctionController<BgrMatrix, GrayMatrix> controller;

        private ViewModelCommand? _InsertCommand;

        public override ViewModelCommand? InsertCommand
        {
            get
            {
                if (_InsertCommand == null)
                {
                    _InsertCommand = new ViewModelCommand(Insert);
                }
                return _InsertCommand;
            }
        }

        public void Insert()
        {
            controller.HandleInsert();
        }

        private ViewModelCommand? _AppendCommand;

        public override ViewModelCommand? AppendCommand
        {
            get
            {
                if (_AppendCommand == null)
                {
                    _AppendCommand = new ViewModelCommand(Append);
                }
                return _AppendCommand;
            }
        }

        public void Append()
        {
            controller.HandleAppend();
        }
    }

    public class BinaryFunctionViewModel : SelectableFunctionViewModel
    {
        public BinaryFunctionViewModel(BinaryFunctionController controller)
        {
            this.controller = controller;
        }

        public override string? FunctionName => "二値化";
        protected override Type GetPreviousType => typeof(GrayMatrix);
        protected override Type GetNextType => typeof(BinaryMatrix);

        private readonly BinaryFunctionController controller;

        private ViewModelCommand? _InsertCommand;

        public override ViewModelCommand? InsertCommand
        {
            get
            {
                if (_InsertCommand == null)
                {
                    _InsertCommand = new ViewModelCommand(Insert);
                }
                return _InsertCommand;
            }
        }

        public void Insert()
        {
            controller.HandleInsert(Threshold);
        }

        private ViewModelCommand? _AppendCommand;

        public override ViewModelCommand? AppendCommand
        {
            get
            {
                if (_AppendCommand == null)
                {
                    _AppendCommand = new ViewModelCommand(Append);
                }
                return _AppendCommand;
            }
        }

        public void Append()
        {
            controller.HandleAppend(Threshold);
        }

        private int _Threshold;

        public int Threshold
        {
            get
            { return _Threshold; }
            set
            {
                if (_Threshold == value)
                    return;
                _Threshold = value;
                RaisePropertyChanged();
            }
        }
    }

    public class FindLargestControurFunctionViewModel : SelectableFunctionViewModel
    {
        public FindLargestControurFunctionViewModel(NoArgFunctionController<BinaryMatrix, Points> controller)
        {
            this.controller = controller;
        }

        public override string? FunctionName => "最も大きい輪郭の検出";
        protected override Type GetPreviousType => typeof(BinaryMatrix);
        protected override Type GetNextType => typeof(Points);

        private readonly NoArgFunctionController<BinaryMatrix, Points> controller;

        private ViewModelCommand? _InsertCommand;

        public override ViewModelCommand? InsertCommand
        {
            get
            {
                if (_InsertCommand == null)
                {
                    _InsertCommand = new ViewModelCommand(Insert);
                }
                return _InsertCommand;
            }
        }

        public void Insert()
        {
            controller.HandleInsert();
        }

        private ViewModelCommand? _AppendCommand;

        public override ViewModelCommand? AppendCommand
        {
            get
            {
                if (_AppendCommand == null)
                {
                    _AppendCommand = new ViewModelCommand(Append);
                }
                return _AppendCommand;
            }
        }

        public void Append()
        {
            controller.HandleAppend();
        }
    }


    public class ApproxFunctionViewModel : SelectableFunctionViewModel
    {
        public ApproxFunctionViewModel(NoArgFunctionController<Points, Points> controller)
        {
            this.controller = controller;
        }

        public override string? FunctionName => "輪郭の直線近似";
        protected override Type GetPreviousType => typeof(Points);
        protected override Type GetNextType => typeof(Points);

        private readonly NoArgFunctionController<Points, Points> controller;

        private ViewModelCommand? _InsertCommand;

        public override ViewModelCommand? InsertCommand
        {
            get
            {
                if (_InsertCommand == null)
                {
                    _InsertCommand = new ViewModelCommand(Insert);
                }
                return _InsertCommand;
            }
        }

        public void Insert()
        {
            controller.HandleInsert();
        }

        private ViewModelCommand? _AppendCommand;

        public override ViewModelCommand? AppendCommand
        {
            get
            {
                if (_AppendCommand == null)
                {
                    _AppendCommand = new ViewModelCommand(Append);
                }
                return _AppendCommand;
            }
        }

        public void Append()
        {
            controller.HandleAppend();
        }
    }

    public class CropFunctionViewModel : SelectableFunctionViewModel
    {
        public CropFunctionViewModel(CropFunctionController controller)
        {
            this.controller = controller;
        }

        public override string? FunctionName => "切り抜き";
        protected override Type GetPreviousType => typeof(BgrMatrix);
        protected override Type GetNextType => typeof(GrayMatrix);

        private readonly CropFunctionController controller;

        private ViewModelCommand? _InsertCommand;

        public override ViewModelCommand? InsertCommand
        {
            get
            {
                if (_InsertCommand == null)
                {
                    _InsertCommand = new ViewModelCommand(Insert);
                }
                return _InsertCommand;
            }
        }

        public void Insert()
        {
            controller.HandleInsert(X, Y, Width, Height);
        }

        private ViewModelCommand? _AppendCommand;

        public override ViewModelCommand? AppendCommand
        {
            get
            {
                if (_AppendCommand == null)
                {
                    _AppendCommand = new ViewModelCommand(Append);
                }
                return _AppendCommand;
            }
        }

        public void Append()
        {
            controller.HandleAppend(X, Y, Width, Height);
        }

        private int _X;

        public int X
        {
            get
            { return _X; }
            set
            {
                if (_X == value)
                    return;
                _X = value;
                RaisePropertyChanged();
            }
        }

        private int _Y;

        public int Y
        {
            get
            { return _Y; }
            set
            {
                if (_Y == value)
                    return;
                _Y = value;
                RaisePropertyChanged();
            }
        }

        private int _Width;

        public int Width
        {
            get
            { return _Width; }
            set
            {
                if (_Width == value)
                    return;
                _Width = value;
                RaisePropertyChanged();
            }
        }

        private int _Height;

        public int Height
        {
            get
            { return _Height; }
            set
            {
                if (_Height == value)
                    return;
                _Height = value;
                RaisePropertyChanged();
            }
        }

    }

    public class ReadImageFunctionViewModel : SelectableFunctionViewModel
    {
        public ReadImageFunctionViewModel(ReadImageFunctionController controller)
        {
            this.controller = controller;
        }

        public override string? FunctionName => "画像の読み込み";
        protected override Type? GetPreviousType => null;
        protected override Type GetNextType => typeof(BgrMatrix);

        private readonly ReadImageFunctionController controller;

        private ViewModelCommand? _InsertCommand;

        public override ViewModelCommand? InsertCommand
        {
            get
            {
                if (_InsertCommand == null)
                {
                    _InsertCommand = new ViewModelCommand(Insert);
                }
                return _InsertCommand;
            }
        }

        public void Insert()
        {
            controller.HandleInsert(Filename);
        }

        private ViewModelCommand? _AppendCommand;

        public override ViewModelCommand? AppendCommand
        {
            get
            {
                if (_AppendCommand == null)
                {
                    _AppendCommand = new ViewModelCommand(Append);
                }
                return _AppendCommand;
            }
        }

        public void Append()
        {
            throw new InvalidOperationException();
        }

        private string? _Filename;

        public string? Filename
        {
            get
            { return _Filename; }
            set
            {
                if (_Filename == value)
                    return;
                _Filename = value;
                RaisePropertyChanged();
            }
        }

        private ViewModelCommand? _SelectFileCommand;

        public ViewModelCommand? SelectFileCommand
        {
            get
            {
                if (_SelectFileCommand == null)
                {
                    _SelectFileCommand = new ViewModelCommand(SelectFile);
                }
                return _SelectFileCommand;
            }
        }

        public void SelectFile()
        {
            var message = new OpeningFileSelectionMessage("SelectFile");
            message.Filter = "PNG|*.png|JPEG|*.jpg;*.jpeg|GIF|*.gif";
            Messenger.Raise(message);

            if (message.Response == null || message.Response.Length == 0)
            {
                return;
            }

            Filename = message.Response[0];
        }

    }
}
