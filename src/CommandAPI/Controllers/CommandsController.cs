using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Models;
using CommandAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using CommandAPI.Dtos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommandAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommandsController : Controller
    {
        private readonly ICommandAPIRepo _commandAPIRepo;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public CommandsController(ICommandAPIRepo commandAPIRepo, IWebHostEnvironment env, IMapper mapper)
        {
            _commandAPIRepo = commandAPIRepo;
            _env = env;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsList()
        {
            IEnumerable<Command> commandsList = await Task.Run(() => _commandAPIRepo.GetAllCommands());

            return Ok(new
            {
                data = _mapper.Map<IEnumerable<CommandReadDto>>(commandsList),
                env = _env.EnvironmentName
            });
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public async Task<ActionResult<CommandReadDto>> GetCommandById(int id)
        {
            Command command = await Task.Run<Command>(() => _commandAPIRepo.GetCommandById(id));

            if (command == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                data = _mapper.Map<CommandReadDto>(command),
                env = _env.EnvironmentName
            });
        }

        [HttpPost]
        public async Task<ActionResult<CommandReadDto>> CreateCommand([FromBody] CommandCreateDto cmd)
        {
            Command newCommand = _mapper.Map<Command>(cmd);

            await Task.Run(() => _commandAPIRepo.CreateCommand(newCommand));
            await Task.Run(() => _commandAPIRepo.SaveChanges());

            CommandReadDto createdCommand = _mapper.Map<CommandReadDto>(newCommand);

            //return Created("Success", new { data = createdCommand });

            return (
                CreatedAtRoute(
                    nameof(GetCommandById),
                    new { Id = createdCommand.Id },
                    new { data = createdCommand }
                )
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = await Task.Run(() => _commandAPIRepo.GetCommandById(id));

            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            // Mapper will mutate commandModelFromRepo in db context
            _mapper.Map(commandUpdateDto, commandModelFromRepo);

            // After muttation, we need to save changes to db
            await Task.Run(() => _commandAPIRepo.SaveChanges());

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommand(int id)
        {
            var commmandToDelete = await Task.Run(() => _commandAPIRepo.GetCommandById(id));

            if (commmandToDelete == null)
            {
                return NotFound();
            }

            await Task.Run(() => _commandAPIRepo.DeleteCommand(commmandToDelete));

            // After muttation, we need to save changes to db
            await Task.Run(() => _commandAPIRepo.SaveChanges());

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            Command commandModelFromRepo = _commandAPIRepo.GetCommandById(id);

            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            CommandUpdateDto commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);

            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);

            _commandAPIRepo.UpdateCommand(commandModelFromRepo);
            _commandAPIRepo.SaveChanges();

            return NoContent();
        }

    }
}

