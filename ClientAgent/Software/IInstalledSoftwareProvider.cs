using Inventory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAgent.Software
{
    public interface IInstalledSoftwareProvider
    {
        public IEnumerable<InstalledSoftware> GetInstalledSoftware();
        public IEnumerable<RunningProgram> GetRunningPrograms();
    }
}
