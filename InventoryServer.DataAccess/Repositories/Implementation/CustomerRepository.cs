using Dapper;
using InventoryServer.DataAccess.Entities;
using InventoryServer.DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace InventoryServer.DataAccess.Repositories.Implementation
{
    public class CustomerRepository : Repository<Customer, int>, ICustomerRepository 
    {
        public CustomerRepository(IDbConnection _db)
            : base(_db)
        {

        }

        public override IEnumerable<Customer> GetAll()
        {
            return db.Query<Customer>("SELECT * FROM Customers").ToList();
        }

        public override Customer Get(int id)
        {
            return db.Query<Customer>("SELECT * FROM Customers WHERE Id = @Id", new { Id = id })
                .FirstOrDefault();
        }

        public override void Add(Customer entity)
        {
            var parameters = new DynamicParameters(entity);
            db.Execute("INSERT INTO Customers(FirstName, LastName, Email) VALUES(@FirstName, @LastName, @Email)", parameters);
        }
    }
}
