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
    public class ClientAgentController : ControllerBase
    {
        private readonly IClientAgentRepository clientAgentRepository;

        public ClientAgentController(IClientAgentRepository _clientAgentRepository)
        {
            clientAgentRepository = _clientAgentRepository;
        }

        [HttpGet]
        public IEnumerable<ClientAgent> Get()
        {
            return clientAgentRepository.GetAll();
        }

        [HttpGet]
        [Route("register/{id}")]
        public bool IsAgentRegistered(Guid id)
        {
            return clientAgentRepository.Exists(id);
        }

        [HttpPost]
        [Route("register")]
        public StatusCodeResult RegisterAgent([FromBody] ClientAgent clientAgent)
        {
            if (string.IsNullOrEmpty(clientAgent.Ip))
            {
                clientAgent.Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            try
            {
                clientAgentRepository.Add(clientAgent);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public ClientAgent GetCustomer(Guid id)
        {
            return clientAgentRepository.Get(id);
        }
    }
}
