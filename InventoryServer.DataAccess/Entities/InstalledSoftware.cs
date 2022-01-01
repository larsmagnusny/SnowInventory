using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryServer.DataAccess.Entities
{
    public class InstalledSoftware
    {
        public Guid ClientAgentId { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Path { get; set; }
        public string License { get; set; }
    }
}
