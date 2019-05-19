using Autofac;
using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Core.Domain.Nodes.Elements.Models;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Configuration;
using BluetoothMesh.Infrastructure.DBL;
using BluetoothMesh.Infrastructure.Handler;
using BluetoothMesh.Infrastructure.Repositories;
using BluetoothMesh.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BluetoothMesh.UI
{
    /// <summary>
    /// Logika interakcji dla klasy ble.xaml
    /// </summary>
    public partial class ble : Window
    {
        public static Autofac.IContainer Container { get; set; }
        public IBluetoothMeshContext mesh;
        public Node clientNode;
        public NodeBearer bearer;
        public ConfigurationClientModel serverModel;
        public ble()
        {
            InitializeComponent();
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

        }

        private void click(object sender, RoutedEventArgs e)
        {
            IssueMessage("Friend", "SET", "2", "Unicast", "7");
        }

        private void IssueMessage(string procedureAnswer, string messageTypeAnswer, string parameterAnswer, string addressTypeAnswer, string addressAnswer)
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
            serverModel.SendMessage(bearer, message);

        }

        static Autofac.IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NodeRepository<Node>>().As<IBaseNodeRepository<Node>>();
            builder.RegisterType<BroadcastService>().As<IBroadcastService>();

            builder.RegisterType<BluetoothMeshContext>().As<IBluetoothMeshContext>().SingleInstance();


            var assembly = typeof(Infrastructure.Commands.ICommand)
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
    }
}
