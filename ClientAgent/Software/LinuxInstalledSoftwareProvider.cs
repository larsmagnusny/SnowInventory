using Inventory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAgent.Software
{
    public class LinuxInstalledSoftwareProvider : IInstalledSoftwareProvider
    {
        public IEnumerable<InstalledSoftware> GetInstalledSoftware()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RunningProgram> GetRunningPrograms()
        {
            throw new NotImplementedException();
        }
    }
}
