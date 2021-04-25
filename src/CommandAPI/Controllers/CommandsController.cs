using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            IEnumerable<string> list = await Task.FromResult<string[]>(new string[] { "this", "is", "hard", "coded", "text" });

            return list;
        }
    }
}

