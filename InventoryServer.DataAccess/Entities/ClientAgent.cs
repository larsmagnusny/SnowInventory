using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryServer.DataAccess.Entities
{
    public class ClientAgent
    {
        public Guid Id { get; set; }
        public string Ip { get; set; }
        public string ComputerName { get; set; }
        public string OSVersion { get; set; }
        public string Motherboard { get; set; }
        public string BIOS { get; set; }
        public long Memory { get; set; }
        public long HDDSpace { get; set; }
    }
}
