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

namespace BluetoothMesh
{
    class Program
    {
        static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            Container = BuildContainer();
            var context = Container.Resolve<IBluetoothMeshContext>();

            foreach (var server in context.NodeServers)
            {
                server.SetDispacher(Container.Resolve<ICommandDispatcher>());
            }

            // testowy request 
            BaseRequest baseRequest = new BaseRequest()
            {
                Heartbeats = 8,
                //Message = "elo kto pl",
                TargetNodeId = 7
            };

            BaseRequest multicastBaseRequestA = new BaseRequest()
            {
                Heartbeats = 8,
                //Message = "multi król",
                TargetNodeId = MulticastProvider.ALL_NODES.GroupId
            };

            BaseRequest multicastBaseRequestB = new BaseRequest()
            {
                Heartbeats = 8,
                //Message = "hehe",
                TargetNodeId = MulticastProvider.KITCHEN.GroupId
            };

            BaseRequest LEBaseRequest = new BaseRequest()
            {
                Heartbeats = 8,
                //Message = "low_energy",
                TargetNodeId = 4
            };

            //context.NodeServers[0].Send(baseRequest);
            //context.NodeServers[0].Send(multicastBaseRequestA);
            //context.NodeServers[0].Send(multicastBaseRequestB);
         //   context.NodeServers[0].Send(LEBaseRequest);

            //to do
            GetRequest getRequest = new GetRequest()
            {
                Heartbeats = 8,
                TargetNodeId = 4
            };
            context.NodeServers[0].Send(getRequest);

            ////todo
            //SetRequest setRequest = new SetRequest()
            //{
            //    Heartbeats = 8,
            //    TargetNodeId = 4
            //};
            //context.NodeServers[0].Send(setRequest);
            ////to do
            //StatusRequest statusRequest = new StatusRequest()
            //{
            //    Heartbeats = 8,
            //    TargetNodeId = 4
            //};
            //context.NodeServers[0].Send(getRequest);



            Console.WriteLine("\nPress any key to close server.");
            Console.ReadKey(true);

        }

        /// <summary>
        /// wrzucamy do magicznego pudełka wszystko co potrzebne
        /// </summary>
        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NodeRepository<Node>>().As<INodeRepository<Node>>();
            builder.RegisterType<BroadcastService>().As<IBroadcastService>();

            builder.RegisterType<BluetoothMeshContext>().As<IBluetoothMeshContext>().SingleInstance();


            var assembly = typeof(ICommand)
                .GetTypeInfo()
                .Assembly;

            //rejestrujemy wszystkie klasy które dziedziczą po ICommandHandler
            builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(ICommandHandler<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<SendHandler<GetRequest>>().As<ICommandHandler<SendCommand<GetRequest>>>();
            builder.RegisterType<SendHandler<SetRequest>>().As<ICommandHandler<SendCommand<SetRequest>>>();
            builder.RegisterType<SendHandler<StatusRequest>>().As<ICommandHandler<SendCommand<StatusRequest>>>();
            builder.RegisterType<SendHandler<BaseRequest>>().As<ICommandHandler<SendCommand<BaseRequest>>>();

            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
       
    }
}
