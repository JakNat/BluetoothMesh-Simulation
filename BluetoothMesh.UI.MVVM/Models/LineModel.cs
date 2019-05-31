
using System.Windows.Media;

namespace BluetoothMesh.UI.MVVM.Models
{
    public class LineModel
    {
        public Brush Stroke { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public double StrokeThickness { get; set; }
    }
}
