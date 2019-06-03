using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Core.Domain.Nodes.Elements.Models;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.DBL;
using BluetoothMesh.UI.MVVM.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BluetoothMesh.UI.MVVM.ViewModels
{
    public class SenderViewModel : Screen
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IBluetoothMeshContext _context;
        private readonly IEventAggregator eventAggregator;

        public SenderViewModel(INodeRepository nodeRepository, IAddressRepository addressRepository, IBluetoothMeshContext context, IEventAggregator eventAggregator)
        {
            _nodeRepository = nodeRepository;
            Nodes = new List<NodeModel>();

            nodeRepository.GetAll().ToList().ForEach(x => Nodes.Add(new NodeModel(x)));
            _addressRepository = addressRepository;
            _context = context;
            this.eventAggregator = eventAggregator;

            SelectedNode = new NodeModel(nodeRepository.Get(1));
            SelectedProcedureType = Procedure.DefaultTTL.ToString();
            SelectedMessageType = MessageType.GET.ToString();

        }

        public List<NodeModel> Nodes { get; set; }

        #region Combobox lists
        public List<string> Procedures { get => Enum.GetNames(typeof(Procedure)).ToList(); }
        public List<string> MessageTypes { get => Enum.GetNames(typeof(MessageType)).ToList(); }
        public List<string> AddressesTypes { get => Enum.GetNames(typeof(AddressType)).ToList(); }
        #region Addresses
        private List<Address> _addreses = null;
        public List<Address> Addresses
        {
            get { return _addreses; }
            set { _addreses = value; }
        }
        #endregion
        #endregion

        #region Selected Items
        public NodeModel SelectedNode { get; set; }
        public string SelectedProcedureType { get; set; }
        #region SelectedMessageType
        private string _selectedMessageType;
        public string SelectedMessageType
        {
            get { return _selectedMessageType; }
            set
            {
                _selectedMessageType = value;
                ParameterBoxVisibility = _selectedMessageType == MessageType.SET.ToString() ? Visibility.Visible : Visibility.Hidden;
                NotifyOfPropertyChange(() => ParameterBoxVisibility);
            }
        }
        #endregion
        
        #region SelectedAddressType
        private string _selectedAddressType;
        public string SelectedAddressType
        {
            get { return _selectedAddressType; }
            set
            {
                _selectedAddressType = value;
                Addresses = SetAddresses(_selectedAddressType);
                NotifyOfPropertyChange(() => Addresses);
            }
        }
        #endregion

        private List<Address> SetAddresses(string selectedAddresType)
        {
            var addresType = (AddressType)Enum.Parse(typeof(AddressType), selectedAddresType);
            switch (addresType)
            {
                case AddressType.Unassigned:
                    break;
                case AddressType.Unicast:
                    return _addressRepository.GetAllUnicast().ToList();
                case AddressType.Virtual:
                    break;
                case AddressType.Group:
                    return _addressRepository.GetAllGroup().ToList();
                default:
                    break;
            }
            return null;
        }
        public Address SelectedAddress { get; set; }
        public int Parameters { get; set; }
        #endregion

        #region ParameterBoxVisibility
        private Visibility _parameterBoxVisibility = Visibility.Hidden;
        public Visibility ParameterBoxVisibility
        {
            get { return _parameterBoxVisibility; }
            set { _parameterBoxVisibility = _selectedMessageType == nameof(MessageType.SET) ? Visibility.Visible : Visibility.Hidden; }
        }
        #endregion

        #region buttons
        public void IssueMessage()
        {
            eventAggregator.PublishOnUIThread(new ClearPathEvent());
            //for (int i = 0; i < 30; i++)
            //{

                var message = new BaseRequest();
                message.Procedure = (Procedure)Enum.Parse(typeof(Procedure), SelectedProcedureType);

                message.MessageType = (MessageType)Enum.Parse(typeof(MessageType), SelectedMessageType);
                message.Parameters = Parameters;
                message.DST = SelectedAddress;

                var n = _nodeRepository.Get(SelectedNode.NodeId);


                ConfigurationClientModel serverModel = (ConfigurationClientModel)n.Elements[ElementType.primary].Models[ModelType.ConfigurationClient];
                serverModel.SendMessage(_context.NodeServers.FirstOrDefault(x => x.Node.Id == SelectedNode.NodeId), message);
            //}
        }
        #endregion
    }
}
