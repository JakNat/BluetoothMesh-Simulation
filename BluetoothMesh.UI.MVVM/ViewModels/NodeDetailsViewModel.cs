using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Handler;
using BluetoothMesh.UI.MVVM.Models;
using Caliburn.Micro;

namespace BluetoothMesh.UI.MVVM.ViewModels
{
    public class NodeDetailsViewModel : Screen
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IEventAggregator eventAggregator;

        public NodeDetailsViewModel(INodeRepository nodeRepository, IEventAggregator eventAggregator)
        {
            _nodeRepository = nodeRepository;
            this.eventAggregator = eventAggregator;
        }

        private EllipseModel _node;

        public EllipseModel Node
        {
            get { return _node; }
            set
            {
                _node = value;
                PositionX = _node.X;
                PositionY = _node.Y;
            }
        }


        private double _positionX;

        public double PositionX
        {
            get { return _positionX; }
            set
            {
                _positionX = value;
                NotifyOfPropertyChange(() => PositionX);
            }
        }

        private double _positionY;

        public double PositionY
        {
            get { return _positionY; }
            set
            {
                _positionY = value;
                NotifyOfPropertyChange(() => PositionY);

            }
        }

        public void SaveChanges()
        {
            var node = _nodeRepository.Get(Node.Id);
            node.Posistion.X = PositionX;
            node.Posistion.Y = PositionY;

            //_nodeRepository.Update(Node);
            eventAggregator.PublishOnUIThread(new NodeUpdate());

        }


    }
}
