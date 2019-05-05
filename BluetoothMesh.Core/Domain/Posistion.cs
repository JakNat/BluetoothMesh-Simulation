using System;

namespace BluetoothMesh.Core.Domain
{
    public class Posistion
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Posistion(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Posistion B)
        {
            var AB = Math.Sqrt(Math.Pow(X - B.X, 2) + Math.Pow(Y - B.Y, 2));
            return AB;
        }
    }
}
