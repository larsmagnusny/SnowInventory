using InventoryServer.DataAccess.Entities;
using Z.Dapper.Plus;

namespace InventoryServer.DataAccess.Configuration
{
    public class ConfigureDapperPlus
    {
        public ConfigureDapperPlus()
        {
            DapperPlusManager.Entity<InstalledSoftware>()
                 .Table("InstalledSoftware")
                 .Key(o => new { o.ClientAgentId, o.Name });
            DapperPlusManager.Entity<RunningProgram>()
                .Table("RunningPrograms")
                .Key(o => new { o.ClientAgentId, o.ProcessId });
        }
    }
}
