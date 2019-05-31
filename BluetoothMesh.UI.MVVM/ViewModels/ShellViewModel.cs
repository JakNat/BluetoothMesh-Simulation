using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.DBL;
using BluetoothMesh.Infrastructure.Repositories;
using BluetoothMesh.UI.MVVM.Models;
using Caliburn.Micro;
using CommonServiceLocator;
using System.Collections.ObjectModel;

namespace BluetoothMesh.UI.MVVM.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public CanvaViewModel CanvaView { get; set; }
        public SenderViewModel SenderView { get; set; }

        public ShellViewModel(CanvaViewModel canvaViewModel, SenderViewModel senderViewModel = null)
        {
            CanvaView = canvaViewModel;
            SenderView = senderViewModel;
        }
    }
}
