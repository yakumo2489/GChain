using GChain.Domain;
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
using System.Windows.Media;

namespace GChain.ViewModels
{
    public class ConnectedFunctionViewModel : ViewModel
    {
        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).

        // This method would be called from View, when ContentRendered event was raised.
        public void Initialize()
        {
        }

        public ConnectedFunctionViewModel()
        {
        }

        public ConnectedFunctionViewModel(
            string functionName,
            string? previous,
            string? next,
            ImageSource preview,
            Brush previousConnectorColor,
            Brush nextConnectorColor)
        {
            FunctionName = functionName;
            PreviousType = previous;
            NextType = next;
            Preview = preview;
            PreviousConnectorColor = previousConnectorColor;
            NextConnectorColor = nextConnectorColor;
        }

        public string? FunctionName { get; }
        public string? PreviousType { get; }
        public string? NextType { get; }
        public ImageSource? Preview { get; }
        public Brush? PreviousConnectorColor { get; }
        public Brush? NextConnectorColor { get; }
    }
}
