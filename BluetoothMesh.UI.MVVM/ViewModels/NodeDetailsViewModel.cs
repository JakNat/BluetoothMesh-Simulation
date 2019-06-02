using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using Caliburn.Micro;

namespace BluetoothMesh.UI.MVVM.ViewModels
{
    public class NodeDetailsViewModel : Screen
    {
        private readonly INodeRepository _nodeRepository;

        public NodeDetailsViewModel(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        public Node Node{ get; set; }

        private double _positionX;

        public double PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }

        private double _positionY;

        public double PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }

        public void SaveChanges()
        {
            Node.Posistion.X = PositionX;
            Node.Posistion.Y = PositionY;
            //_nodeRepository.Update(Node);
        }


    }
}
