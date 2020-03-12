using System.Threading.Tasks;
using ContosoUniversity.Domain.UniversityAggregate;
using ContosoUniversity.Features.Instructors;
using ContosoUniversity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstructorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SchoolContext _context;

        public InstructorsController(IMediator mediator, SchoolContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet]
        public Task<Instructor[]> GetAllAsync()
        {
            return _context.Instructors.ToArrayAsync();
        }

        [HttpGet("{id}/delete")]
        public Task<Delete.Command> DeleteAsync([FromRoute] Delete.Query query)
        {
            return _mediator.Send(query);
        }

        [HttpPost("delete")]
        public Task DeleteAsync(Delete.Command command)
        {
            return _mediator.Send(command);
        }

        [HttpGet("create")]
        public Task<CreateEdit.Command> CreateAsync()
        {
            return _mediator.Send(new CreateEdit.Query());
        }

        [HttpPost("create")]
        public Task CreateAsync(CreateEdit.Command command)
        {
            return _mediator.Send(command);
        }

        [HttpGet("{id}/edit")]
        public Task<CreateEdit.Command> EditAsync([FromRoute] CreateEdit.Query query)
        {
            return _mediator.Send(query);
        }

        [HttpPost("edit")]
        public Task EditAsync(CreateEdit.Command command)
        {
            return _mediator.Send(command);
        }

        [HttpGet("{id}/details")]
        public Task<Details.Model> DetailsAsync([FromRoute] Details.Query query)
        {
            return _mediator.Send(query);
        }

        [HttpGet("index")]
        public Task<Index.Model> IndexAsync([FromQuery] Index.Query query)
        {
            return _mediator.Send(query);
        }
    }
}