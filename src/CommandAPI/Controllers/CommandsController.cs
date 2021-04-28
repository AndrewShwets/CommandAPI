using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Models;
using CommandAPI.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommandAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo commandAPIRepo;

        public CommandsController(ICommandAPIRepo commandAPIRepo)
        {
            this.commandAPIRepo = commandAPIRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Command>> GetCommandsList()
        {
            IEnumerable<Command> list = await Task.FromResult(commandAPIRepo.GetAllCommands());

            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Command>> GetCommandById(int id)
        {
            Command command = await Task.FromResult<Command>(commandAPIRepo.GetCommandById(id));

            if (command == null)
            {
                return NotFound();
            }

            return Ok(command);
        }
    }
}

