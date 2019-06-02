using Autofac;
using Autofac.Extras.CommonServiceLocator;
using BluetoothMesh.Core.Domain.Models;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.DBL;
using BluetoothMesh.Infrastructure.Handler;
using BluetoothMesh.Infrastructure.Repositories;
using BluetoothMesh.Infrastructure.Services;
using BluetoothMesh.UI.MVVM.ViewModels;
using Caliburn.Micro;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace BluetoothMesh.UI.MVVM
{
    public class AppBootstrapper : BootstrapperBase
    {
        private static IContainer Container;


        public AppBootstrapper()
        {
            this.Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            //ViewModels
            builder.RegisterType<ShellViewModel>();
            builder.RegisterType<CanvaViewModel>();
            builder.RegisterType<SenderViewModel>();

            builder.RegisterType<WindowManager>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<EventAggregator>()
                .AsImplementedInterfaces()
                .SingleInstance();


            builder.RegisterType<BluetoothMeshContext>().As<IBluetoothMeshContext>().SingleInstance();

            builder.RegisterType<NodeRepository>().As<INodeRepository>();
            builder.RegisterType<ElementRepository>().As<IElementRepository>();
            builder.RegisterType<ModelRepository>().As<IModelRepository<Model>>();
            builder.RegisterType<AddressRepository>().As<IAddressRepository>();

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

            Container = container;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(service);
            return Container.Resolve(type) as IEnumerable<object>;
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (Container.IsRegistered(service))
                    return Container.Resolve(service);
            }
            else
            {
                if (Container.IsRegisteredWithKey(key, service))
                    return Container.ResolveKeyed(key, service);
            }

            var msgFormat = "Could not locate any instances of contract {0}.";
            var msg = string.Format(msgFormat, key ?? service.Name);
            throw new Exception(msg);
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
