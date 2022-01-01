using InventoryServer.DataAccess.Entities;
using System;

namespace InventoryServer.DataAccess.Repositories.Interfaces
{
    public interface IClientAgentRepository : IRepository<ClientAgent, Guid>
    {
        public bool Exists(Guid id);   
    }
}
