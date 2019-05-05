using System.Collections.Generic;

namespace BluetoothMesh.Core.Domain
{
    public static class GroupAddressesProvider
    {
        public static Dictionary<string, Address> Dictionary { get; set; }

        public static void SeedList()
        {
            Dictionary = new Dictionary<string, Address>
            {
                { GroupAddresses.AllNodes, new Address(AddressType.Group, 0xFFFF) },
                { GroupAddresses.AllRelays, new Address(AddressType.Group, 0xFFFE) },
                { GroupAddresses.AllFriends, new Address(AddressType.Group, 0xFFFD) },
                { GroupAddresses.AllProxies, new Address(AddressType.Group, 0xFFFC) },
                { GroupAddresses.AllLights, new Address(AddressType.Group, 0xFFFB) },
                { GroupAddresses.StatusResponses, new Address(AddressType.Group, 0xFFFA) },
            };
        }

    }

    public static class GroupAddresses
        {
        public static string AllNodes = "ALL-NODES";
        public static string StatusResponses = "StatusResponse";

        public static string AllFriends = "AllFriends";
        public static string AllRelays = "AllRelays";
        public static string AllProxies = "AllPRoxies";

        public static string AllLights = "AllLights";
    }

}
