using InventoryServer.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace InventoryServer.DataAccess.Repositories.Interfaces
{
    public interface IClientAgentRepository : IRepository<ClientAgent, Guid>
    {
        public bool Exists(Guid id);

        public void UpdateInstalledPrograms(Guid Id, IDictionary<string, InstalledSoftware> installedSoftware);
        public void UpdateRunningPrograms(Guid id, IDictionary<long, RunningProgram> runningPrograms);
    }
}
