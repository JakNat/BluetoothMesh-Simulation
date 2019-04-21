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
                Heartbeats = 3,
                Message = "elo kto pl",
                TargetNodeId = 7
            };

            context.NodeServers[0].Send(baseRequest);

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
