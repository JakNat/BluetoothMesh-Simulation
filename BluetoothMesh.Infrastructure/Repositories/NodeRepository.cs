﻿using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.DBL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BluetoothMesh.Infrastructure.Repositories
{
    /// <summary>
    /// Generyczne repo dla nodów 
    /// </summary>
    public class NodeRepository<T> : INodeRepository<T> where T : Node
    {
        private readonly IBluetoothMeshContext context;

        public NodeRepository(IBluetoothMeshContext context)
        {
            this.context = context;
        }

        public void Add(T baseNode)
        {
            context.BaseNodes.ToList().Add(baseNode);
        }

        public T Get(int id)
        {
            return (T)context.BaseNodes.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return (IEnumerable<T>)context.BaseNodes;
        }

        public IEnumerable<T> GetAllInRange(T baseNode)
        {
            return (IEnumerable<T>)context.BaseNodes
                .Where(x => x.Posistion.DistanceTo(baseNode.Posistion) <= baseNode.Range
                && x.Id != baseNode.Id);
        }

        public IEnumerable<T> GetAllSubscribed()
        {
            throw new NotImplementedException();
        }
    }
}