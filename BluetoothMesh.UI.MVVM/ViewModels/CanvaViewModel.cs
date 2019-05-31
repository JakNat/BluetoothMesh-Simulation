using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Handler;
using BluetoothMesh.UI.MVVM.Models;
using BluetoothMesh.UI.Providers;
using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace BluetoothMesh.UI.MVVM.ViewModels
{
    public class CanvaViewModel : Screen, IHandle<NodeUpdate>
    {
        #region privates
        private readonly INodeRepository _nodeRepository;
        private ObservableCollection<EllipseModel> _myNodes;
        private ObservableCollection<LineModel> _myLines;
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region public
        public ObservableCollection<EllipseModel> MyNodes
        {
            get { return _myNodes; }
            set
            {
                _myNodes = value;
                NotifyOfPropertyChange(() => MyNodes);
            }
        }
        public ObservableCollection<TextBlockModel> MyTextBlocks { get; set; }
        public ObservableCollection<LineModel> MyLines
        {
            get { return _myLines; }
            set
            {
                _myLines = value;
                NotifyOfPropertyChange(() => MyLines);
            }
        }
        public ObservableCollection<TextBlockModel> TextBlockModels { get; set; }
        #endregion

        public CanvaViewModel(INodeRepository nodeRepository, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _nodeRepository = nodeRepository;
            _eventAggregator.Subscribe(this);
            MyNodes = new ObservableCollection<EllipseModel>();
            TextBlockModels = new ObservableCollection<TextBlockModel>();
            foreach (var node in _nodeRepository.GetAll())
            {
                var mynode = new EllipseModel(node);
                TextBlockModel textBlock = new TextBlockModel
                {
                    Text = node.Id.ToString(),
                    X = node.Posistion.X * 10 - mynode.Height / 6,
                    Y = node.Posistion.Y * 10 - mynode.Height / 3
                };
                TextBlockModels.Add(textBlock);

                mynode.Fill = LayoutProvider.ColorPicker(node);
                MyNodes.Add(mynode);
            }
            seedMyLines();
        }

        public void seedMyLines()
        {
            MyLines = new ObservableCollection<LineModel>();
            for (int i = 0; i <= 50; i++)
            {
                LineModel line = new LineModel
                {
                    Stroke = Brushes.Gray,
                    X1 = i * 10,
                    X2 = i * 10,
                    Y1 = 0,
                    Y2 = 300,

                    StrokeThickness = 0.5
                };
                MyLines.Add(line);
            }

            for (int i = 0; i <= 30; i++)
            {
                LineModel line = new LineModel
                {
                    Stroke = Brushes.Gray,

                    X1 = 0,
                    X2 = 500,
                    Y1 = i * 10,
                    Y2 = i * 10,

                    StrokeThickness = 0.5
                };
                MyLines.Add(line);
            }
        }

        public void NodeClicked(EllipseModel myNode)
        {

        }

        public void Handle(NodeUpdate message)
        {
            MyNodes = new ObservableCollection<EllipseModel>();
            TextBlockModels = new ObservableCollection<TextBlockModel>();
            foreach (var node in _nodeRepository.GetAll())
            {
                var mynode = new EllipseModel(node);
                TextBlockModel textBlock = new TextBlockModel
                {
                    Text = node.Id.ToString(),
                    X = node.Posistion.X * 10 - mynode.Height / 6,
                    Y = node.Posistion.Y * 10 - mynode.Height / 3
                };
                TextBlockModels.Add(textBlock);

                mynode.Fill = LayoutProvider.ColorPicker(node);
                if (message.NodeId == node.Id)
                {
                    mynode.Height += 5;
                    mynode.Width += 5;
                }
                MyNodes.Add(mynode);
            }
            seedMyLines();
            var nodeFrom = MyNodes.FirstOrDefault(x => x.Name == Convert.ToString(message.From.Value));
            var nodeTo = MyNodes.FirstOrDefault(x => x.Name == Convert.ToString(message.NodeId));

            //var nodeFrom = _nodeRepository.GetAll().FirstOrDefault(x => x.Address.Value == message.From.Value);
            //var nodeTo = _nodeRepository.GetAll().First(x => x.Id == message.NodeId);
            LineModel line = new LineModel
            {
                Stroke = Brushes.Gray,

                X1 = nodeFrom.X + (nodeFrom.Width / 2),
                Y1 = nodeFrom.Y + (nodeFrom.Height / 2),
                X2 = nodeTo.X + (nodeTo.Width / 2),
                Y2 = nodeTo.Y + (nodeTo.Height / 2),

                StrokeThickness = 3
            };
            MyLines.Add(line);
        }
    }
}
