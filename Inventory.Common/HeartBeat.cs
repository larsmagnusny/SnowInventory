using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common
{
    public class HeartBeat
    {
        public Guid ClientAgentId { get; set; }
        public IEnumerable<InstalledSoftware> InstalledSoftware { get; set; }
        public IEnumerable<RunningProgram> RunningPrograms { get; set; }
    }
}
