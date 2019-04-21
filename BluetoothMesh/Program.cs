using Autofac;
using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Configuration;
using BluetoothMesh.Infrastructure.DBL;
using BluetoothMesh.Infrastructure.Repositories;
using BluetoothMesh.Infrastructure.Services;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Reflection;

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
                Message = "elo kto pl",
                TargetNodeId = 7
            };

            BaseRequest multicastBaseRequestA = new BaseRequest()
            {
                Heartbeats = 8,
                Message = "multi król",
                TargetNodeId = MulticastProvider.ALL_NODES.GroupId
            };

            BaseRequest multicastBaseRequestB = new BaseRequest()
            {
                Heartbeats = 8,
                Message = "hehe",
                TargetNodeId = MulticastProvider.KITCHEN.GroupId
            };

            BaseRequest LEBaseRequest = new BaseRequest()
            {
                Heartbeats = 8,
                Message = "low_energy",
                TargetNodeId = 4
            };

            context.NodeServers[0].Send(baseRequest);
            context.NodeServers[0].Send(multicastBaseRequestA);
            context.NodeServers[0].Send(multicastBaseRequestB);
            context.NodeServers[0].Send(LEBaseRequest);

            Console.WriteLine("\nPress any key to close server.");
            Console.ReadKey(true);

        }

        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<BaseNodeRepository<BaseNode>>().As<IBaseNodeRepository<BaseNode>>();
            builder.RegisterType<BroadcastService>().As<IBroadcastService>();

            builder.RegisterType<BluetoothMeshContext>().As<IBluetoothMeshContext>().SingleInstance();

            var assembly = typeof(ICommand)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(ICommandHandler<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
       
    }
}
