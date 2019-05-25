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
using BluetoothMesh.Core.Domain.Elements;
using System.Linq;
using BluetoothMesh.Core.Domain.Models;
using BluetoothMesh.Core.Domain.Nodes.Elements.Models;
using System.Threading.Tasks;
using CommonServiceLocator;
using Autofac.Extras.CommonServiceLocator;
using System.Diagnostics;

namespace BluetoothMesh
{
    class Program
    {
        static IContainer Container { get; set; } = BuildContainer();

        static void Main(string[] args)
        {
            Container = BuildContainer();
            
            var context = Container.Resolve<IBluetoothMeshContext>();

            Node clientNode = context.Nodes.ToList()[0];
            var bearer = context.NodeServers[0];
            ConfigurationClientModel serverModel = (ConfigurationClientModel)clientNode.Elements[ElementType.primary].Models[ModelType.ConfigurationClient];

            while (true)    
            {
                Console.WriteLine("-------------MENU:--------------\n" +
                    "1) Display all nodes\n" +
                    "2) Send message");

                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        foreach (var node in context.Nodes)
                        {
                            Console.WriteLine($"\n-Node nr {node.Id}");
                            Console.WriteLine($"-Node features:");
                            if (node.ConfigurationServerModel.CompositionData.IsRelay)
                                Console.WriteLine($"--Relay");

                            if (node.ConfigurationServerModel.CompositionData.IsFriend)
                                Console.WriteLine($"--Friend");

                            if (node.ConfigurationServerModel.CompositionData.IsProxy)
                                Console.WriteLine($"--Proxy");

                            Console.WriteLine("---Node elements:");
                            foreach (var elemnt in node.Elements)
                            {
                                Console.WriteLine($"---Element nr {elemnt.Value.Address} --- ");
                                Console.WriteLine($"---Element type {elemnt.Key.ToString()}");
                                foreach (var model in elemnt.Value.Models)
                                {
                                    //Console.WriteLine($"-----Model nr {model.Value.Address} --- ");
                                    Console.WriteLine($"-----Model type {model.Key.ToString()}");
                                    Console.WriteLine($"-----Model procedures:");

                                    model.Value.Procedures.ForEach(p => Console.WriteLine($"------{p.ToString()}"));
                                }
                            }
                        }
                        break;
                    case 2:
                        var message = new BaseRequest();
                        Console.WriteLine("\nWhat type of procedure\n");
                        foreach (string volume in Enum.GetNames(typeof(Procedure)))
                        {
                            Console.Write($"{volume} ");
                        }
                        Console.Write(":\n");
                        Enum.TryParse(Console.ReadLine(), out Procedure procedure);
                        message.Procedure = procedure;

                        Console.WriteLine("\nWhat type of message");
                        foreach (string volume in Enum.GetNames(typeof(MessageType)))
                        {
                            Console.Write($"{volume} ");
                        }
                        string messsageTyoeAnswer = Console.ReadLine();
                        Enum.TryParse(messsageTyoeAnswer, out MessageType messageType);
                        message.MessageType = messageType;
                        if (MessageType.SET == messageType)
                        {
                            Console.WriteLine("Parameter:");
                            message.Parameters = Convert.ToInt32(Console.ReadLine());
                        }

                        Console.WriteLine("\nWhat type of address\n");
                        foreach (string volume in Enum.GetNames(typeof(AddressType)))
                        {
                            Console.Write($"{volume} ");
                        }
                        string addressAnswer = Console.ReadLine();
                        Console.WriteLine(addressAnswer);
                        Enum.TryParse(addressAnswer, out AddressType addressType);
                        int addressValue = 0;
                        switch (addressType)
                        {
                            case AddressType.Unassigned:
                                break;
                            case AddressType.Unicast:
                                Console.WriteLine("\nWhich one:\n");
                                foreach (var node in context.Nodes)
                                {
                                    Console.WriteLine($"- {node.Id} : {node.Address.Value}");
                                }
                                addressValue = Convert.ToInt32(Console.ReadLine());
                                message.DST = context.Nodes.Select(x => x.Address).FirstOrDefault(x => x.Value == addressValue);
                                break;
                            case AddressType.Virtual:
                                //Console.WriteLine("\n  gl :V   \n");
                                //var a = context.Nodes.SelectMany(x => x.Elements.Values);
                                //foreach (var model in context.Nodes.SelectMany(x => x.Elements.Values).SelectMany(x => x.Models.Values))
                                //{
                                //    Console.WriteLine($"- {model.Address.GuidId} : {model.Address.Value}");
                                //}
                                //addressValue = Convert.ToInt32(Console.ReadLine());
                                //message.DST = context.Nodes.Select(x => x.Address).FirstOrDefault(x => x.Value == addressValue);
                                break;
                            case AddressType.Group:
                                Console.WriteLine("\nWhich one:\n");
                                foreach (var address in GroupAddressesProvider.Dictionary)
                                {
                                    Console.WriteLine($"- {address.Key} : {address.Value.Value}");
                                }
                                addressValue = Convert.ToInt32(Console.ReadLine());
                                message.DST = GroupAddressesProvider.Dictionary.Values.FirstOrDefault(x => x.Value == addressValue);
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine(message.ToString());
                        

                        serverModel.SendMessage(bearer,message);

                        break;
                }

                //finish all threads
                Process thisProc = Process.GetCurrentProcess();
                ProcessThreadCollection mythreads = thisProc.Threads;
            }

        }

        /// <summary>
        /// wrzucamy do magicznego pudełka wszystko co potrzebne
        /// </summary>
        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<BluetoothMeshContext>().As<IBluetoothMeshContext>().SingleInstance();

            builder.RegisterType<NodeRepository>().As<INodeRepository>();
            builder.RegisterType<ElementRepository>().As<IElementRepository>();
            builder.RegisterType<ModelRepository>().As<IModelRepository<Model>>();

            builder.RegisterType<BroadcastService>().As<IBroadcastService>();

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

            var container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));

            return container;
        }
       
    }
}
