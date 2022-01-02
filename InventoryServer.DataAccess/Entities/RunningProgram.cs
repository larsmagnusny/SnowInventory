using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryServer.DataAccess.Entities
{
    public class RunningProgram
    {
        public Guid ClientAgentId { get; set; }
        public long ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string WindowTitle { get; set; }
    }
}
