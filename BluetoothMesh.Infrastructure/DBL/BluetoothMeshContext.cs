using BluetoothMesh.Core.Domain;
using BluetoothMesh.Infrastructure.Configuration;
using System.Collections.Generic;
using System.Linq;
using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Core.Domain.Models;
using BluetoothMesh.Core.Domain.Nodes.Elements.Models;

namespace BluetoothMesh.Infrastructure.DBL
{
    public class BluetoothMeshContext : IBluetoothMeshContext
    {
        public BluetoothMeshContext()
        {
            GroupAddressesProvider.SeedList();

            Nodes = new List<Node>()
            {
                new Node(1,new Posistion(1,1), new Features(){ Relay =  false, Proxy = true, Friend = false}, GetClient()),
                new Node(2,new Posistion(2,10), new Features(){ Relay = false, Proxy = false, Friend = false}, GetLightBulb()),
                new Node(3,new Posistion(8,6), new Features(){ Relay =  true, Proxy = false, Friend = false}),
                new Node(4,new Posistion(3,13), new Features(){ Relay = false, Proxy = false, Friend = false }),
                new Node(5,new Posistion(14,10), new Features(){ Relay =false, Proxy = false, Friend = false}, GetLightBulb()),
                new Node(6,new Posistion(16,18), new Features(){ Relay =true, Proxy = false, Friend = false}),
                new Node(7,new Posistion(24,16), new Features(){ Relay =false, Proxy = false, Friend = false }, GetLightBulb()),
                new Node(8,new Posistion(36,18), new Features(){ Relay =true, Proxy = false, Friend = false}),
                new Node(9,new Posistion(46,18), new Features(){ Relay =false, Proxy = false, Friend = false}),

            };

            NodeServers = new List<NodeBearer>();
            foreach (var node in Nodes)
            {
                var nodeServer = new NodeBearer(node);
                NodeServers.Add(nodeServer);
            }

            Nodes[0].Elements[ElementType.primary].Models[ModelType.ConfigurationServer].Procedures.Clear();
            #region default subscriptions

            Elements.Where(x => x.Models.ContainsKey(ModelType.Light)).ToList().ForEach(x =>
            x.Subscriptions.Add(GroupAddressesProvider.Dictionary[GroupAddresses.AllLights]));

            foreach (var element in Elements.Where(x => x.Models.ContainsKey(ModelType.ConfigurationServer)))
            {
                ConfigurationServerModel configurationServer = (ConfigurationServerModel)element.Models[ModelType.ConfigurationServer];
                if (configurationServer != null)
                {
                    if (configurationServer.CompositionData.IsFriend)
                        element.Subscriptions.Add(GroupAddressesProvider.Dictionary[GroupAddresses.AllFriends]);
                    if (configurationServer.CompositionData.IsRelay)
                        element.Subscriptions.Add(GroupAddressesProvider.Dictionary[GroupAddresses.AllRelays]);
                    if (configurationServer.CompositionData.IsProxy)
                        element.Subscriptions.Add(GroupAddressesProvider.Dictionary[GroupAddresses.AllProxies]);
                    element.Subscriptions.Add(GroupAddressesProvider.Dictionary[GroupAddresses.AllNodes]);
                }
            }
            #endregion
        }

        private static Dictionary<ModelType, Model> GetClient()
        {
            return new Dictionary<ModelType, Model>()
            {
                {ModelType.ConfigurationClient, new ConfigurationClientModel() } 
            };
        }

        private static Dictionary<ModelType, Model> GetLightBulb()
        {
            return new Dictionary<ModelType, Model>()
            {
                {ModelType.Light, new LightModel() }
            };
        }

        public List<NodeBearer> NodeServers { get; set; }
        public List<Node> Nodes { get; set; }
        public IEnumerable<Element> Elements { get { return Nodes.SelectMany(x => x.Elements.Values); } }
        public IEnumerable<Model> Models { get { return Elements.SelectMany(x => x.Models.Values); } }
    }
}
