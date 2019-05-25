using System.Collections.Generic;
using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Core.Domain.Models;
using BluetoothMesh.Infrastructure.Configuration;

namespace BluetoothMesh.Infrastructure.DBL
{
    public interface IBluetoothMeshContext
    {
        IEnumerable<Element> Elements { get; }
        IEnumerable<Model> Models { get; }
        List<Node> Nodes { get; set; }
        List<NodeBearer> NodeServers { get; set; }
    }
}