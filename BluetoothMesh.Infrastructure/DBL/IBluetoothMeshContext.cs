using System.Collections.Generic;
using BluetoothMesh.Core.Domain;
using BluetoothMesh.Infrastructure.Configuration;

namespace BluetoothMesh.Infrastructure.DBL
{
    public interface IBluetoothMeshContext
    {
        List<BaseNode> BaseNodes { get; set; }
        List<NodeServer> NodeServers { get; set; }
        int NodeCount { get; set; }
    }
}