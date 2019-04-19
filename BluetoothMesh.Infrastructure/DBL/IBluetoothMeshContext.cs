using System.Collections.Generic;
using BluetoothMesh.Core.Domain;

namespace BluetoothMesh.Infrastructure.DBL
{
    public interface IBluetoothMeshContext
    {
        IEnumerable<BaseNode> BaseNodes { get; set; }
        int NodeCount { get; set; }
    }
}