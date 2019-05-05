using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Domain.Models.States
{
    /* 4.2.1
     * The Composition Data state contains information about a node, the elements it includes,
     * and the supported models.
     * The Composition Data is composed of a number of pages of information.
     */
    public class CompositionData
    {
        /// <summary>
        /// 4.3
        /// </summary>
        public ushort Features { get; set; }
    }
}
