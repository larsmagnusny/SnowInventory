using InventoryServer.DataAccess.Entities;
using InventoryServer.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository _customerRepository)
        {
            customerRepository = _customerRepository;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return customerRepository.GetAll();
        }
        [HttpPost]
        public StatusCodeResult Post([FromBody] Customer customer)
        {
            try
            {
                customerRepository.Add(customer);
            } catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        public Customer GetCustomer(int id)
        {
            return customerRepository.Get(id);
        }
    }
}
