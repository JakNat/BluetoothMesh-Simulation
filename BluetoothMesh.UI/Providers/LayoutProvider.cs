using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Nodes.Elements.Models;
using BluetoothMesh.Infrastructure.Configuration;
using BluetoothMesh.Infrastructure.DBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BluetoothMesh.UI.Providers
{
    public class LayoutProvider
    {

        private static int CircleSize = 25;
        private static int LineThiccccness = 2;

        public static List<KeyValuePair<Node, Ellipse>> NodeIcons = new List<KeyValuePair<Node, Ellipse>>();
        public List<int> NodesWithMessages = new List<int>();
        public List<int> ReceivingNodes = new List<int>();
        public static void DrawNodes(Canvas canvas, IBluetoothMeshContext mesh)
        {
            foreach (Node node in mesh.Nodes)
            {
                Ellipse ellipse = new Ellipse() { Width = CircleSize, Height = CircleSize, Fill = Brushes.White };
                NodeIcons.Add(new KeyValuePair<Node, Ellipse>(node, ellipse));
                ellipse.MouseDown += Ellipse_Click;
                ellipse.Name = "e" + Convert.ToString(node.Id);
                canvas.Children.Add(ellipse);

                Canvas.SetLeft(ellipse, node.Posistion.X * 10 - ellipse.Width / 2);
                Canvas.SetTop(ellipse, node.Posistion.Y * 10 - ellipse.Height / 2);

            }
        }

        public static void Ellipse_Click(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            NodeDetailsWindow newWindow = new NodeDetailsWindow
            {
                Node = GetNodeById(Int32.Parse(new string(ellipse.Name[1], 1)))
            };
            newWindow.Show();
            e.Handled = true;
        }

        public static void ColorNodes()
        {
            foreach (var pair in NodeIcons)
            {
                pair.Value.Fill = ColorPicker(pair.Key);
            }
        }

        public static void SignNodes(Canvas canvas, IBluetoothMeshContext mesh)
        {
            foreach (Node node in mesh.Nodes)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = node.Id.ToString()
                };

                Canvas.SetLeft(textBlock, node.Posistion.X * 10 - CircleSize / 6);
                Canvas.SetTop(textBlock, node.Posistion.Y * 10 - CircleSize / 3);
                canvas.Children.Add(textBlock);
            }
        }

        public static void DrawConnections(Canvas canvas, IBluetoothMeshContext mesh)
        {
            foreach (Node node in mesh.Nodes)
            {
                IEnumerable<Node> nodesInRange = GetAllInRange(node, mesh);
                foreach (Node nodeInRange in nodesInRange)
                {
                    Line line = new Line
                    {
                        Stroke = Brushes.Black,

                        X1 = node.Posistion.X * 10,
                        X2 = nodeInRange.Posistion.X * 10,
                        Y1 = node.Posistion.Y * 10,
                        Y2 = nodeInRange.Posistion.Y * 10,

                        StrokeThickness = LineThiccccness
                    };
                    canvas.Children.Add(line);
                }
            }
        }


        public static Brush ColorPicker(Node node)
        {
            Color friend = Colors.BlueViolet;
            Color proxy = Colors.LightCoral;
            Color relay = Colors.Gold + Colors.Bisque;
            int colorFlag = 0;
            if (node.ConfigurationServerModel.Friend) { colorFlag = colorFlag + 2; }
            if (node.ConfigurationServerModel.GATTProxy) { colorFlag = colorFlag + 4; }
            if (node.ConfigurationServerModel.Relay) { colorFlag = colorFlag + 8; }

            
            switch (colorFlag)
            {
                case 0:
                    return Brushes.White;
                case 2:
                    return Brushes.BlueViolet;
                case 4:
                    return Brushes.LightCoral;
                case 8:
                    return Brushes.Gold;
                case 6:
                    //var brush = new System.Windows.Media.LinearGradientBrush();
                    //brush.GradientStops.Add(new GradientStop(friend, 0.0));
                    //brush.GradientStops.Add(new GradientStop(proxy, 1.0));
                    //return brush;
                    return Brushes.Cyan;
                case 10:
                    //var brush = new System.Windows.Media.LinearGradientBrush();
                    //brush.GradientStops.Add(new GradientStop(friend, 0.0));
                    //brush.GradientStops.Add(new GradientStop(relay, 1.0));
                    //return brush;
                    return Brushes.YellowGreen;
                case 12:
                    //brush = new System.Windows.Media.LinearGradientBrush();
                    //brush.GradientStops.Add(new GradientStop(proxy, 0.0));
                    //brush.GradientStops.Add(new GradientStop(relay, 1.0));
                    return Brushes.PowderBlue;
                case 14:
                    //brush = new System.Windows.Media.LinearGradientBrush();
                    //brush.GradientStops.Add(new GradientStop(friend, 0.0));
                    //brush.GradientStops.Add(new GradientStop(proxy, 0.5));
                    //brush.GradientStops.Add(new GradientStop(relay, 1.0));
                    //return brush;
                    return Brushes.Aqua;
                default:
                    return Brushes.White;
            }
        }

        public static void DrawGrid(Canvas canvas)
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

        private static List<Node> GetAllInRange(Node baseNode, IBluetoothMeshContext mesh)
        {
            List<Node> result = new List<Node>();
            foreach (Node node in mesh.Nodes)
            {
                if (node.Posistion.DistanceTo(baseNode.Posistion) <= baseNode.Range && node.Id != baseNode.Id)
                {
                    result.Add(node);
                }
            }
            return result;
        }

        private static Node GetNodeByPosition(int X, int Y)
        {
            foreach (var pair in NodeIcons)
            {
                if (pair.Key.Posistion.X == X && pair.Key.Posistion.Y == Y) { return pair.Key; }
            }
            return null;
        }

        private static Node GetNodeById(int id)
        {
            foreach (var pair in NodeIcons)
            {
                if (pair.Key.Id == id) { return pair.Key; }
            }
            return null;
        }

        private static DrawingBrush CreateMulticolorBrush()
        {
            DrawingBrush myBrush = new DrawingBrush();

            GeometryDrawing backgroundSquare =
                new GeometryDrawing(
                    Brushes.White,
                    null,
                    new RectangleGeometry(new Rect(0, 0, 100, 100)));

            GeometryGroup aGeometryGroup = new GeometryGroup();
            aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(0, 0, 50, 50)));
            aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(50, 50, 50, 50)));

            LinearGradientBrush checkerBrush = new LinearGradientBrush();
            checkerBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.0));
            checkerBrush.GradientStops.Add(new GradientStop(Colors.Gray, 1.0));

            GeometryDrawing checkers = new GeometryDrawing(checkerBrush, null, aGeometryGroup);

            DrawingGroup checkersDrawingGroup = new DrawingGroup();
            checkersDrawingGroup.Children.Add(backgroundSquare);
            checkersDrawingGroup.Children.Add(checkers);

            myBrush.Drawing = checkersDrawingGroup;
            myBrush.Viewport = new Rect(0, 0, 0.25, 0.25);
            myBrush.TileMode = TileMode.Tile;

            return myBrush;
        }
    }
}
