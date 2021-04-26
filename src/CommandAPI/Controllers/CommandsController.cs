using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommandAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<string>> GetCommandsList()
        {
            IEnumerable<string> list = await Task.FromResult(new string[] { "this", "is", "hard", "coded", "text" });

            return list;
        }

        [HttpPost]
        public async Task CreateCommand([FromBody] Command command)
        {
            Console.WriteLine($"Id: {command.Id}.\nHowTo: {command.HowTo}.\nCommandLine: {command.CommandLine}.");
        }
    }
}

