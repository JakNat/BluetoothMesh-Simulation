﻿using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Handler;
using BluetoothMesh.UI.MVVM.Models;
using BluetoothMesh.UI.Providers;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace BluetoothMesh.UI.MVVM.ViewModels
{
    public class CanvaViewModel : Screen, IHandle<NodeUpdate>, IHandle<ClearPathEvent>
    {
        public static Node PickedNode;

        #region privates
        private readonly INodeRepository _nodeRepository;
        private ObservableCollection<EllipseModel> _myNodes;
        private ObservableCollection<LineModel> _myLines;
        private readonly IEventAggregator _eventAggregator;
        private readonly NodeDetailsViewModel _nodeDetailsViewModel;

        private List<LineModel> path;
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

        public CanvaViewModel(INodeRepository nodeRepository, IEventAggregator eventAggregator, NodeDetailsViewModel nodeDetailsViewModel)
        {
            _eventAggregator = eventAggregator;
            _nodeDetailsViewModel = nodeDetailsViewModel;
            _nodeRepository = nodeRepository;
            _eventAggregator.Subscribe(this);
            path = new List<LineModel>();
            Draw();
        }

        private void Draw()
        {
            MyNodes = new ObservableCollection<EllipseModel>();
            TextBlockModels = new ObservableCollection<TextBlockModel>();

            foreach (var node in _nodeRepository.GetAll())
            {

                var mynode = new EllipseModel(node);
                var x = _nodeRepository.GetAll();
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

                foreach (var item in path)
                {
                    MyLines.Add(item);
                }
            }
        }

        public void NodeClicked(EllipseModel myNode)
        {
            IWindowManager manager = new WindowManager();
            _nodeDetailsViewModel.Node = myNode;
            PickedNode = _nodeRepository.Get(myNode.Id);
            
            //manager.ShowWindow(_nodeDetailsViewModel, null, null);
        }


        public void Handle(NodeUpdate message)
        {
            Draw();
            //System.Threading.Thread.Sleep(10);

            if (message.From != null)
            {

                var actualNode = MyNodes.FirstOrDefault(x => x.Id == message.NodeId);
                actualNode.Height += 5;
                actualNode.Width += 5;

                var nodeFrom = MyNodes.FirstOrDefault(x => x.Name == Convert.ToString(message.From.Value));
                var nodeTo = MyNodes.FirstOrDefault(x => x.Name == Convert.ToString(message.NodeId));

                LineModel line = new LineModel
                {
                    Stroke = Brushes.Gray,

                    X1 = nodeFrom.X + (nodeFrom.Width / 2),
                    Y1 = nodeFrom.Y + (nodeFrom.Height / 2),
                    X2 = nodeTo.X + (nodeTo.Width / 2),
                    Y2 = nodeTo.Y + (nodeTo.Height / 2),

                    StrokeThickness = 3
                };
                path.Add(line);
                MyLines.Add(line);
            }
        }

        public void Handle(ClearPathEvent message)
        {
            path.Clear();
        }
    }
}
