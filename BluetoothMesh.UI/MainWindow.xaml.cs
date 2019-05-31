using Autofac;
using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.DBL;
using BluetoothMesh.Infrastructure.Repositories;
using BluetoothMesh.Infrastructure.Services;
using System;
using System.Reflection;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Handler;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using BluetoothMesh.Core.Domain.Nodes.Elements.Models;
using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Infrastructure.Configuration;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using BluetoothMesh.UI.Providers;

namespace BluetoothMesh.UI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {            
        public static Autofac.IContainer Container { get; set; }
        public IBluetoothMeshContext mesh;
        public Node clientNode;
        public NodeBearer bearer;
        public ConfigurationClientModel serverModel;
        
        public List<int> NodesWithMessages = new List<int>();
        public List<int> ReceivingNodes = new List<int>();
        
        public MainWindow()
        {
            GroupAddressesProvider.SeedList();
            Container = BuildContainer();
            mesh = Container.Resolve<IBluetoothMeshContext>();

            foreach (var server in mesh.NodeServers)
            {
                server.SetDispacher(Container.Resolve<ICommandDispatcher>());
            }

            Node clientNode = mesh.Nodes[0];
            bearer = mesh.NodeServers.ToList().FirstOrDefault(x => x.Node.Id == 1);
            serverModel = (ConfigurationClientModel)clientNode.Elements[ElementType.primary].Models[ModelType.ConfigurationClient];
            
            InitializeComponent();

            Providers.LayoutProvider.DrawGrid(canvas);
            Providers.LayoutProvider.DrawGrid(canvas);
            Providers.LayoutProvider.DrawConnections(canvas, mesh);
            Providers.LayoutProvider.DrawNodes(canvas, mesh);
            Providers.LayoutProvider.ColorNodes();
            Providers.LayoutProvider.SignNodes(canvas, mesh);

            Setup();            
        }

        


        static Autofac.IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NodeRepository<Node>>().As<IBaseNodeRepository<Node>>();
            builder.RegisterType<BroadcastService>().As<IBroadcastService>();

            builder.RegisterType<BluetoothMeshContext>().As<IBluetoothMeshContext>().SingleInstance();


            var assembly = typeof(ICommand)
                .GetTypeInfo()
                .Assembly;

            //rejestrujemy wszystkie klasy które dziedziczą po ICommandHandler
            builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(ICommandHandler<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<SendHandler<BaseRequest>>().As<ICommandHandler<SendCommand<BaseRequest>>>();

            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }



        private Node GetNodeById(int id)
        {
            foreach (Node node in mesh.Nodes)
            {
                if (node.Id == id) { return node; }
            }
            return null;
        }

        string parameter;
        private void IssueMessage_Click(object sender, RoutedEventArgs e)
        {
            var issuingNode = IssuingNode.SelectedItem as string;
            var procedureType = ProcedureTypes.SelectedItem as string;
            var messageType = MessageTypes.SelectedItem as string;
            parameter = Parameter.Text;
            var addressType = AddressTypes.SelectedItem as string;
            var address = Addresses.SelectedItem as string;
            Console.WriteLine(procedureType + "\n" + messageType + "\n" + parameter + "\n" + addressType + "\n" + address);
            //IssueMessage("1", "Friend", "SET", "2", "Unicast", "7");
            IssueMessage(issuingNode, procedureType, messageType, parameter, addressType, address);
            
            
        }

        private void IssueMessage(string issuingNodeAnswer, string procedureAnswer, string messageTypeAnswer, string parameterAnswer, string addressTypeAnswer, string addressAnswer)
        {
            var message = new BaseRequest();
            Enum.TryParse(procedureAnswer, out Procedure procedure);
            message.Procedure = procedure;

            Enum.TryParse(messageTypeAnswer, out MessageType messageType);
            message.MessageType = messageType;
            message.Parameters = Convert.ToInt32(parameterAnswer);
            
            Enum.TryParse(addressAnswer, out AddressType addressType);
            int addressValue = 0;
            switch (addressType)
            {
                case AddressType.Unassigned:
                    break;
                case AddressType.Unicast:
                    addressValue = Convert.ToInt32(addressAnswer);
                    message.DST = mesh.Nodes.Select(x => x.Address).FirstOrDefault(x => x.Value == addressValue);
                    break;
                case AddressType.Virtual:
                    var a = mesh.Nodes.SelectMany(x => x.Elements.Values);
                    addressValue = Convert.ToInt32(addressAnswer);
                    message.DST = mesh.Nodes.Select(x => x.Address).FirstOrDefault(x => x.Value == addressValue);
                    break;
                case AddressType.Group:
                    addressValue = Convert.ToInt32(addressAnswer);
                    message.DST = GroupAddressesProvider.Dictionary.Values.FirstOrDefault(x => x.Value == addressValue);
                    break;
                default:
                    break;
            }

            int issuingNode = Int32.Parse(issuingNodeAnswer);
            Node issuer = GetNodeById(issuingNode);
            issuer.StatusFlag = 1;
            bearer = mesh.NodeServers.ToList().FirstOrDefault(x => x.Node.Id == issuingNode);
            serverModel.SendMessage(bearer, message);

        }

        void Node_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            if (Int32.Parse(e.PropertyName) % 10 == 1)
            {
                NodesWithMessages.Add((int)(Int32.Parse(e.PropertyName)) / 10);
            }
            if (Int32.Parse(e.PropertyName) % 10 == 2)
            {
                ReceivingNodes.Add((int)(Int32.Parse(e.PropertyName)) / 10);
            }

        }
        
        private void DispatcherTimerSetter()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1500)
            };
            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            Providers.LayoutProvider.ColorNodes();

            foreach (var x in Providers.LayoutProvider.NodeIcons)
            {
                if (NodesWithMessages.Contains(x.Key.Id)) { x.Value.Fill = Brushes.Red; }
                if (ReceivingNodes.Contains(x.Key.Id)) { x.Value.Fill = Brushes.LawnGreen; }
            }
            NodesWithMessages.Clear();
            ReceivingNodes.Clear();
        }

        void Setup()
        {
            foreach (Node node in mesh.Nodes)
            {
                node.PropertyChanged += Node_PropertyChanged;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimerSetter();
        }

        private void ProcedureTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxProvider.ProcedureTypeSetter(sender);
        }

        private void MessageTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxProvider.MessageTypeSetter(sender);
        }

        private void AddressTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxProvider.AddressTypeSetter(sender);
        }

        private void AddressTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboboxProvider.AddressSetter(sender, Addresses, textDestination, mesh);
        }

        private void IssuingNodeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxProvider.IssuingNodeSetter(sender, mesh);
        }
    }
}
