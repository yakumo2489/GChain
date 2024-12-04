using GChain.Adaptors;
using GChain.Applications;
using GChain.Domain;
using GChain.Infrastructure;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GChain.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private GenerateCodeController generateCodeController;

        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public void Initialize()
        {
        }

        public MainWindowViewModel()
        {
            var chainStore = new ChainStore();

            var functionPresenter = new FunctionPresenter();
            functionPresenter.ViewModel = this;
            var readImageFunctionUsecase = new InsertEntryPointInteractor(
                functionPresenter,
                chainStore);
            var readImageFunctionController = new ReadImageFunctionController(readImageFunctionUsecase);

            var insertGrayUsecase = new InsertChainInteractor<BgrMatrix, GrayMatrix>(
                functionPresenter,
                chainStore);
            var appendGrayUsecase = new AppendChainInteractor<BgrMatrix, GrayMatrix>(
                functionPresenter,
                chainStore,
                chainStore);
            var grayFunctionController = new NoArgFunctionController<BgrMatrix, GrayMatrix>(
                () => new Gray(),
                insertGrayUsecase,
                appendGrayUsecase);

            var insertBinaryUsecase = new InsertChainInteractor<GrayMatrix, BinaryMatrix>(
                functionPresenter,
                chainStore);
            var appendBinaryUsecase = new AppendChainInteractor<GrayMatrix, BinaryMatrix>(
                functionPresenter,
                chainStore,
                chainStore);
            var binaryFunctionController = new BinaryFunctionController(
                insertBinaryUsecase,
                appendBinaryUsecase);

            var insertFindLargestContourUsecase = new InsertChainInteractor<BinaryMatrix, Points>(
                functionPresenter,
                chainStore);
            var appendFindLargestContourUsecase = new AppendChainInteractor<BinaryMatrix, Points>(
                functionPresenter,
                chainStore,
                chainStore);
            var findLargestContourFunctionController = new NoArgFunctionController<BinaryMatrix, Points>(
                () => new FindLargestCountours(),
                insertFindLargestContourUsecase,
                appendFindLargestContourUsecase);

            var insertAproxUsecase = new InsertChainInteractor<Points, Points>(
                functionPresenter,
                chainStore);
            var appendApproxUsecase = new AppendChainInteractor<Points, Points>(
                functionPresenter,
                chainStore,
                chainStore);
            var approxFunctionController = new NoArgFunctionController<Points, Points>(
                () => new Approx(),
                insertAproxUsecase,
                appendApproxUsecase);

            var insertCropUsecase = new InsertChainInteractor<BgrMatrix, BgrMatrix>(
                functionPresenter,
                chainStore);
            var appendCropUsecase = new AppendChainInteractor<BgrMatrix, BgrMatrix>(
                functionPresenter,
                chainStore,
                chainStore);
            var cropFunctionController = new CropFunctionController(
                insertCropUsecase,
                appendCropUsecase);

            var generateCodePresenter = new GenerateCodePresenter();
            var generateCodeUsecase = new GenerateCodeInteractor(generateCodePresenter, chainStore);
            generateCodeController = new GenerateCodeController(generateCodeUsecase);

            SelectableFunctions = new List<SelectableFunctionViewModel>()
            {
                new ReadImageFunctionViewModel(readImageFunctionController),
                new GrayFunctionViewModel(grayFunctionController),
                new BinaryFunctionViewModel(binaryFunctionController),
                new FindLargestControurFunctionViewModel(findLargestContourFunctionController),
                new ApproxFunctionViewModel(approxFunctionController),
                new CropFunctionViewModel(cropFunctionController)
            };
        }

        private ObservableSynchronizedCollection<ConnectedFunctionViewModel> _ConnectedFunctions = new ObservableSynchronizedCollection<ConnectedFunctionViewModel>();

        public ObservableSynchronizedCollection<ConnectedFunctionViewModel> ConnectedFunctions
        {
            get
            { return _ConnectedFunctions; }
            set
            {
                if (_ConnectedFunctions == value)
                    return;
                _ConnectedFunctions = value;
                RaisePropertyChanged();
            }
        }

        public List<SelectableFunctionViewModel> SelectableFunctions { get; }

        private ConnectedFunctionViewModel? _HeadFunction;

        public ConnectedFunctionViewModel? HeadFunction
        {
            get
            { return _HeadFunction; }
            set
            {
                if (_HeadFunction == value)
                    return;
                _HeadFunction = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectableHeads));
                RaisePropertyChanged(nameof(SelectableTails));
            }
        }

        private ConnectedFunctionViewModel? _TailFunction;

        public ConnectedFunctionViewModel? TailFunction
        {
            get
            { return _TailFunction; }
            set
            {
                if (_TailFunction == value)
                    return;
                _TailFunction = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectableHeads));
                RaisePropertyChanged(nameof(SelectableTails));
            }
        }

        public List<SelectableFunctionViewModel> SelectableHeads
            => SelectableFunctions.Where(f => f.NextType == HeadFunction?.PreviousType).ToList();

        private SelectableFunctionViewModel? _SelectedHead;

        public SelectableFunctionViewModel? SelectedHead
        {
            get
            { return _SelectedHead; }
            set
            {
                if (_SelectedHead == value)
                    return;
                _SelectedHead = value;
                RaisePropertyChanged();
            }
        }

        public List<SelectableFunctionViewModel> SelectableTails
            => SelectableFunctions.Where(f => f.PreviousType == TailFunction?.NextType).ToList();

        private SelectableFunctionViewModel? _SelectedTail;

        public SelectableFunctionViewModel? SelectedTail
        {
            get
            { return _SelectedTail; }
            set
            {
                if (_SelectedTail == value)
                    return;
                _SelectedTail = value;
                RaisePropertyChanged();
            }
        }

        private ConnectedFunctionViewModel? _ViewingFunction;

        public ConnectedFunctionViewModel? ViewingFunction
        {
            get
            { return _ViewingFunction; }
            set
            {
                if (_ViewingFunction == value)
                    return;
                _ViewingFunction = value;
                RaisePropertyChanged();
            }
        }

        private SelectableFunctionViewModel? _SelectedFirstFunction;

        public SelectableFunctionViewModel? SelectedFirstFunction
        {
            get
            { return _SelectedFirstFunction; }
            set
            {
                if (_SelectedFirstFunction == value)
                    return;
                _SelectedFirstFunction = value;
                RaisePropertyChanged();
            }
        }


        public bool IsFunctionEmpty => ConnectedFunctions.Count == 0;
        public bool CanInsert => ConnectedFunctions.FirstOrDefault()?.PreviousType != null;
        public bool CanAppend => ConnectedFunctions.LastOrDefault()?.NextType != null;

        public void InsertConnectedFunction(ConnectedFunctionViewModel vm)
        {
            ConnectedFunctions.Insert(0, vm);
            RaisePropertyChanged(nameof(IsFunctionEmpty));
            RaisePropertyChanged(nameof(CanInsert));
            RaisePropertyChanged(nameof(CanAppend));

            HeadFunction = vm;
            if (TailFunction == null)
            {
                TailFunction = vm;
            }
            ViewingFunction = vm;
        }

        public void AppendConnectedFunction(ConnectedFunctionViewModel vm)
        {
            ConnectedFunctions.Add(vm);
            RaisePropertyChanged(nameof(IsFunctionEmpty));
            RaisePropertyChanged(nameof(CanInsert));
            RaisePropertyChanged(nameof(CanAppend));

            TailFunction = vm;
            if (HeadFunction == null)
            {
                HeadFunction = vm;
            }
            ViewingFunction = vm;
        }

        private ViewModelCommand? _GenerateCodeCommand;

        public ViewModelCommand? GenerateCodeCommand
        {
            get
            {
                if (_GenerateCodeCommand == null)
                {
                    _GenerateCodeCommand = new ViewModelCommand(GenerateCode);
                }
                return _GenerateCodeCommand;
            }
        }

        public void GenerateCode()
        {
            var message = new SavingFileSelectionMessage("InputSaveFilename");
            message.Title = "コード生成";
            message.Filter = "Python|*.py";

            Messenger.Raise(message);
            if (message.Response == null || message.Response.Length == 0)
            {
                return;
            }

            generateCodeController.Handle(message.Response[0]);
        }

    }
}
