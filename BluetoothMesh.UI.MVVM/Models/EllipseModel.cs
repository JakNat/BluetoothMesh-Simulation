using BluetoothMesh.Core.Domain;
using System;
using System.Windows.Media;

namespace BluetoothMesh.UI.MVVM.Models
{
    public class EllipseModel
    {
        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Brush Fill { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public EllipseModel(Node node)
        {
            Id = node.Id;
            Width = 25;
            Height = 25;
            Fill = Brushes.White;
            Name = Convert.ToString(node.Id);
            X = node.Posistion.X * 10 - Width / 2;
            Y = node.Posistion.Y * 10 - Height / 2;
        }
    }
}
