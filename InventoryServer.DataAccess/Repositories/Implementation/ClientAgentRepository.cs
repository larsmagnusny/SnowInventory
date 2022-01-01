using Dapper;
using InventoryServer.DataAccess.Entities;
using InventoryServer.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace InventoryServer.DataAccess.Repositories.Implementation
{
    public class ClientAgentRepository : Repository<ClientAgent, Guid>, IClientAgentRepository 
    {
        public ClientAgentRepository(IDbConnection _db)
            : base(_db)
        {

        }

        public override IEnumerable<ClientAgent> GetAll()
        {
            return db.Query<ClientAgent>("SELECT * FROM ClientAgents").ToList();
        }

        public override ClientAgent Get(Guid id)
        {
            return db.Query<ClientAgent>("SELECT * FROM ClientAgents WHERE Id = @Id", new { Id = id })
                .FirstOrDefault();
        }

        public override void Add(ClientAgent entity)
        {
            var parameters = new DynamicParameters(entity);
            db.Execute("INSERT INTO ClientAgents(Id, Ip, ComputerName, OSVersion, Motherboard, BIOS, Memory, HDDSpace) VALUES(@Id, @Ip, @ComputerName, @OSVersion, @Motherboard, @BIOS, @Memory, @HDDSpace)", parameters);
        }

        public bool Exists(Guid id)
        {
            var agent = db.Query<ClientAgent>("SELECT Id FROM ClientAgents WHERE Id = @Id", new { Id = id }).FirstOrDefault();

            if(agent != null)
            {
                return agent.Id == id;
            }

            return false;
        }
    }
}
