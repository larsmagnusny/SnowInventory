using System;

namespace InventoryServer.DataAccess.Entities
{
    public class InstalledSoftware
    {
        public Guid ClientAgentId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Publisher { get; set; }
    }
}
