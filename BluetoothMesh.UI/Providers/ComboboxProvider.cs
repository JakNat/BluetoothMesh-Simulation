using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.DBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BluetoothMesh.UI.Providers
{
    public class ComboboxProvider
    {
        public static void IssuingNodeSetter(object sender, IBluetoothMeshContext context)
        {
            var combobox = sender as ComboBox;
            List<string> dataList = new List<string>();
            foreach (var node in context.Nodes)
            {
                dataList.Add(node.Id.ToString());
            }
            combobox.ItemsSource = dataList;
            combobox.SelectedIndex = 0;
        }
        public static void ProcedureTypeSetter(object sender)
        {
            var dataList = Enum.GetNames(typeof(Procedure));
            var list = new List<string>(dataList);
            var combobox = sender as ComboBox;
            combobox.ItemsSource = list;
            combobox.SelectedIndex = 0;
        }

        public static void MessageTypeSetter(object sender)
        {
            var dataList = Enum.GetNames(typeof(MessageType));
            var list = new List<string>(dataList);
            var combobox = sender as ComboBox;
            combobox.ItemsSource = list;
            combobox.SelectedIndex = 0;
        }

        public static void AddressTypeSetter(object sender)
        {
            var dataList = Enum.GetNames(typeof(AddressType));
            var list = new List<string>(dataList);
            var combobox = sender as ComboBox;
            combobox.ItemsSource = list;
            combobox.SelectedIndex = 0;
        }

        public static void AddressSetter(object sender, ComboBox target, TextBox textTarget, IBluetoothMeshContext context)
        {
            var comboBox = sender as ComboBox;
            string address = comboBox.SelectedItem as string;
            Enum.TryParse(address, out AddressType addressType);
            List<string> dataList;

            switch (addressType)
            {
                case AddressType.Unassigned:
                    textTarget.Visibility = System.Windows.Visibility.Hidden;
                    target.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case AddressType.Unicast:
                    dataList = new List<string>();
                    foreach (var node in context.Nodes)
                    {
                        dataList.Add(node.Id.ToString());
                    }
                    textTarget.Visibility = System.Windows.Visibility.Visible;
                    target.Visibility = System.Windows.Visibility.Visible;
                    target.ItemsSource = dataList;
                    target.SelectedIndex = 0;
                    break;
                case AddressType.Virtual:
                    dataList = new List<string>();
                    foreach (var model in context.Nodes.SelectMany(x => x.Elements.Values).SelectMany(x => x.Models.Values))
                    {
                        dataList.Add(model.Address.GuidId.ToString());
                    }
                    textTarget.Visibility = System.Windows.Visibility.Visible;
                    target.Visibility = System.Windows.Visibility.Visible;
                    target.ItemsSource = dataList;
                    target.SelectedIndex = 0;
                    break;
                case AddressType.Group:
                    dataList = new List<string>();
                    foreach (var addr in GroupAddressesProvider.Dictionary)
                    {
                        dataList.Add(addr.Key);
                    }
                    textTarget.Visibility = System.Windows.Visibility.Visible;
                    target.Visibility = System.Windows.Visibility.Visible;
                    target.ItemsSource = dataList;
                    target.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }
    }
}
