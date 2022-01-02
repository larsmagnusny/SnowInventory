using Dapper;
using InventoryServer.DataAccess.Entities;
using InventoryServer.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Z.Dapper.Plus;

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
            db.Execute("INSERT INTO ClientAgents(Id, Ip, ComputerName, OSVersion, Motherboard, BIOS, Memory, HDDSpace) VALUES(@Id, @Ip, @ComputerName, @OSVersion, @Motherboard, @BIOS, @Memory, @HDDSpace);", parameters);
        }

        public override void Update(ClientAgent entity)
        {
            var parameters = new DynamicParameters(entity);
            db.Execute("UPDATE ClientAgents SET Ip = @Ip, ComputerName = @ComputerName, OSVersion = @OSVersion, Motherboard = @Motherboard, BIOS = @BIOS, Memory = @Memory, HDDSpace = @HDDSpace WHERE Id = @Id;", parameters);
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

        public void UpdateInstalledPrograms(Guid id, IDictionary<string, InstalledSoftware> installedSoftware)
        {
            var existing = new HashSet<string>(db.Query<string>("SELECT Name FROM InstalledSoftware WHERE ClientAgentId = @Id", new { Id = id }));

            List<InstalledSoftware> toInsert = new List<InstalledSoftware>();
            List<InstalledSoftware> toUpdate = new List<InstalledSoftware>();
            List<InstalledSoftware> toRemove = new List<InstalledSoftware>();

            foreach(var item in installedSoftware)
            {
                if (existing.Contains(item.Value.Name))
                    toUpdate.Add(item.Value);
                else
                    toInsert.Add(item.Value);
            }

            foreach(var item in existing)
            {
                if (!installedSoftware.ContainsKey(item))
                    toRemove.Add(new InstalledSoftware { ClientAgentId = id, Name = item });
            }

            db.BulkInsert(toInsert);
            db.BulkUpdate(toUpdate);
            db.BulkDelete(toRemove);
        }

        public void UpdateRunningPrograms(Guid id, IDictionary<long, RunningProgram> runningPrograms)
        {
            var existing = new HashSet<long>(db.Query<long>("SELECT ProcessId FROM RunningPrograms WHERE ClientAgentId = @Id", new { Id = id }));

            List<RunningProgram> toInsert = new List<RunningProgram>();
            List<RunningProgram> toUpdate = new List<RunningProgram>();
            List<RunningProgram> toRemove = new List<RunningProgram>();

            foreach (var item in runningPrograms)
            {
                if (existing.Contains(item.Value.ProcessId))
                    toUpdate.Add(item.Value);
                else
                    toInsert.Add(item.Value);
            }

            foreach (var item in existing)
            {
                if (!runningPrograms.ContainsKey(item))
                    toRemove.Add(new RunningProgram { ClientAgentId = id, ProcessId = item });
            }

            db.BulkInsert(toInsert);
            db.BulkUpdate(toUpdate);
            db.BulkDelete(toRemove);
        }
    }
}
