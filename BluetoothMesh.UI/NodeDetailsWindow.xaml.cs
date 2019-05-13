using BluetoothMesh.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class NodeDetailsWindow : Window
    {
        public Node Node;
        public NodeDetailsWindow()
        {
            InitializeComponent();
            Console.WriteLine();
            //Providers.NodeDetailLayoutProvider.DrawNode(Node, canvas);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Providers.NodeDetailLayoutProvider.DrawNode(Node, canvas);
            Providers.NodeDetailLayoutProvider.DrawElements(Node, canvas);
        }
    }
    
}
