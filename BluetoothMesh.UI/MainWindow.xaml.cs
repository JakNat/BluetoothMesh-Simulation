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

namespace BluetoothMesh.UI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static IContainer Container { get; set; }
        public IBluetoothMeshContext mesh;
        

        int CircleSize = 25;
        int LineThiccccness = 2;

          
        public MainWindow()
        {
            Container = BuildContainer();
            mesh = Container.Resolve<IBluetoothMeshContext>();
            InitializeComponent();

            //DrawGrid();
            //DrawConnections();
            //DrawNodes();
            //SignNodes();

            //ChangeNodeColorOnMessage();
        }

        private void DrawNodes()
        {
            foreach (Node node in mesh.Nodes)
            {
                Ellipse ellipse = new Ellipse() { Width = CircleSize, Height = CircleSize, Fill = Brushes.Blue };
                canvas.Children.Add(ellipse);

                Canvas.SetLeft(ellipse, node.Posistion.X * 10 - ellipse.Width / 2);
                Canvas.SetTop(ellipse, node.Posistion.Y * 10 - ellipse.Height / 2);

            }
        }

        private void SignNodes()
        {
            foreach (Node node in mesh.Nodes)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = node.Id.ToString();

                Canvas.SetLeft(textBlock, node.Posistion.X * 10 - CircleSize / 6);
                Canvas.SetTop(textBlock, node.Posistion.Y * 10 - CircleSize / 3);
                canvas.Children.Add(textBlock);
            }
        }

        private void DrawConnections()
        {
            foreach (Node node in mesh.Nodes)
            {
                IEnumerable<Node> nodesInRange = mesh.GetAllInRange(node);
                foreach (Node nodeInRange in nodesInRange)
                {
                    Line line = new Line();
                    line.Stroke = Brushes.Black;

                    line.X1 = node.Posistion.X * 10;
                    line.X2 = nodeInRange.Posistion.X * 10;
                    line.Y1 = node.Posistion.Y * 10;
                    line.Y2 = nodeInRange.Posistion.Y * 10;

                    line.StrokeThickness = LineThiccccness;
                    canvas.Children.Add(line);
                }
            }
        }

        private Brush ColorPicker(Features features)
        {
            int colorFlag = 0;
            if (features.Friend) { colorFlag = colorFlag + 2; }
            if (features.Proxy) { colorFlag = colorFlag + 4; }
            if (features.Relay) { colorFlag = colorFlag + 8; }

            switch (colorFlag)
            {
                case 0:
                    return Brushes.White;
                case 2:
                    return Brushes.Aquamarine;
                case 4:
                    return Brushes.Gold;
                case 8:
                    return Brushes.GreenYellow;
                case 6:
                    System.Windows.Media.LinearGradientBrush brush = new System.Windows.Media.LinearGradientBrush();
                    brush.GradientStops.Add(new GradientStop(Colors.Aquamarine, 0.0));
                    brush.GradientStops.Add(new GradientStop(Colors.Gold, 1.0));
                    return brush;
                default:
                    return Brushes.White;
            }
        }

        private void DrawGrid()
        {
            for (int i = 0; i <= 50; i++)
            {
                Line line = new Line
                {
                    Stroke = Brushes.Gray,

                    X1 = i * 10,
                    X2 = i * 10,
                    Y1 = 0,
                    Y2 = 300,

                    StrokeThickness = 0.5
                };
                canvas.Children.Add(line);
            }

            for (int i = 0; i <= 30; i++)
            {
                Line line = new Line
                {
                    Stroke = Brushes.Gray,

                    X1 = 0,
                    X2 = 500,
                    Y1 = i * 10,
                    Y2 = i * 10,

                    StrokeThickness = 0.5
                };
                canvas.Children.Add(line);
            }
        }



        //private void ChangeNodeColorOnMessage()
        //{
        //    foreach (Node node in mesh.Nodes)
        //    {
        //        if (node.stateFlag == 1)
        //        {
        //            Ellipse ellipse = new Ellipse() { Width = CircleSize, Height = CircleSize, Fill = Brushes.Red };
        //            canvas.Children.Add(ellipse);

        //            Canvas.SetLeft(ellipse, node.Posistion.X * 10 - ellipse.Width / 2);
        //            Canvas.SetTop(ellipse, node.Posistion.Y * 10 - ellipse.Height / 2);
        //        }
        //    }
        //}

        //private void IssueMessage_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageIssuer();
        //}


        //    private void MessageIssuer()
        //    {
        //        BaseRequest baseRequest = new BaseRequest()
        //        {
        //            Heartbeats = 8,
        //            //Message = "elo kto pl",
        //            TargetNodeId = 7
        //        };
        //        //context.NodeServers[0].Send(baseRequest);
        //    }
        static IContainer BuildContainer()
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


    }
}
