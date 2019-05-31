using BluetoothMesh.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BluetoothMesh.UI.Providers
{
    class NodeDetailLayoutProvider
    {
        public static void DrawNode(Node node, Canvas canvas)
        {
            canvas.Background = LayoutProvider.ColorPicker(node);
            TextBlock textBlock = new TextBlock
            {
                Text = "Node " + node.Id.ToString(),
                FontSize = 20
            };

            TextBlock textBlockDetails = new TextBlock
            {
                Text = "Node features: \n" + FeatureStringProvider(node),
                FontSize = 12,
                TextAlignment = System.Windows.TextAlignment.Center,
                Width = 400
            };


            Canvas.SetLeft(textBlock, (canvas.ActualWidth/2 - 30));
            Canvas.SetTop(textBlock, 20);
            canvas.Children.Add(textBlock);
            Canvas.SetLeft(textBlockDetails, (canvas.ActualWidth/2 - textBlockDetails.Width/2));
            Canvas.SetTop(textBlockDetails, 45);
            canvas.Children.Add(textBlockDetails);
        }

        public static void DrawElements(Node node, Canvas canvas)
        {
            List<Rectangle> elementIcons = new List<Rectangle>();
            List<TextBox> elementLabels = new List<TextBox>();
            List<List<Rectangle>> modelIcons = new List<List<Rectangle>>();
            List<List<TextBox>> modelLabels = new List<List<TextBox>>();

            int length = node.Elements.Count();
            int width = (450 - 50 - (length - 1) * 5) / length;
            List<int> modelWidth = new List<int>();

            foreach (var element in node.Elements)
            {
                TextBox textBox = new TextBox
                {
                    Text = "Element " + element.Value.Address + "\nElement type: " + element.Key.ToString(),
                    FontSize = 12,
                    TextAlignment = System.Windows.TextAlignment.Center,
                    Width = width - 10,
                    Height = 200
                };
                elementLabels.Add(textBox);

                Rectangle rectangle = new Rectangle
                {
                    Width = width,
                    Height = 200,
                    Fill = Brushes.White
                };
                elementIcons.Add(rectangle);

                int modelLength = element.Value.Models.Count();
                int currModelWidth = (width - 6 - (modelLength - 1) * 3) / modelLength;
                modelWidth.Add(currModelWidth);
                List<Rectangle> currElementModelIcons = new List<Rectangle>();
                List<TextBox> currElementModelLabels = new List<TextBox>();
                foreach (var model in element.Value.Models)
                {
                    TextBox textBoxModel = new TextBox
                    {
                        Text = "Model: \n" + model.Value.Address + "\nModel type: " + model.Key.ToString() +
                                    "\n \n Model procedures: \n" + ModelProceduresProvider(model),
                        FontSize = 10,
                        TextAlignment = System.Windows.TextAlignment.Center,
                        Width = currModelWidth - 10,
                        Height = 150
                    };
                    currElementModelLabels.Add(textBoxModel);

                    Rectangle rectangleModel = new Rectangle
                    {
                        Width = currModelWidth,
                        Height = 150,
                        Fill = Brushes.Green
                    };
                    currElementModelIcons.Add(rectangleModel);
                }
                modelIcons.Add(currElementModelIcons);
                modelLabels.Add(currElementModelLabels);
            }

            for (int i=0; i<elementIcons.Count(); i++)
            {
                //var rectangle = elementIcons[i];
                //canvas.Children.Add(rectangle);
                //Canvas.SetLeft(rectangle, 25 + (i*width + 5*i));
                //Canvas.SetTop(rectangle, 80);

                var textBox = elementLabels[i];
                canvas.Children.Add(textBox);
                Canvas.SetLeft(textBox, 25 + (i * width + 5 * i) + 3);
                Canvas.SetTop(textBox, 85);
                
                for (int j=0; j<modelIcons[i].Count(); j++)
                {
                    //var modelRectangle = modelIcons[i][j];
                    //canvas.Children.Add(modelRectangle);
                    //Canvas.SetLeft(modelRectangle, 25 + (i * width + 5 * i) + 3 + (j * modelWidth[i] + 3 * j));
                    //Canvas.SetTop(modelRectangle, 120);

                    var modelTextBox = modelLabels[i][j];
                    canvas.Children.Add(modelTextBox);
                    Canvas.SetLeft(modelTextBox, 25 + (i * width + 5 * i) + 3 + (j * modelWidth[i] + 3 * j + 3));
                    Canvas.SetTop(modelTextBox, 125);
                }
            }
        }

        private static string FeatureStringProvider(Node node)
        {
            string output = "";
            if (node.ConfigurationServerModel.Relay)
                output += "Relay ";

            if (node.ConfigurationServerModel.Friend)
                output += "Friend ";

            if (node.ConfigurationServerModel.GATTProxy)
                output += "Proxy ";
            return output;
        }

        private static string ModelProceduresProvider(KeyValuePair<Core.Domain.Elements.ModelType, Core.Domain.Models.Model> model)
        {
            string output = "";
            foreach (var procedure in model.Value.Procedures)
            {
                output += procedure.ToString() + "\n";
            }
            return output;
        }


    }
}
