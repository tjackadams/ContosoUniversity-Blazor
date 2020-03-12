using System.Threading.Tasks;
using ContosoUniversity.Features.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("index")]
        public Task<Index.Result> GetAllAsync()
        {
            return _mediator.Send(new Index.Query());
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

        [HttpGet("{id}/details")]
        public Task<Details.Command> DetailsAsync([FromRoute] Details.Query query)
        {
            return _mediator.Send(query);
        }

        [HttpPost("details")]
        public Task DetailsAsync(Details.Command command)
        {
            return _mediator.Send(command);
        }

        [HttpPost]
        public Task CreateAsync(Create.Command command)
        {
            return _mediator.Send(command);
        }

        [HttpGet("{id}/edit")]
        public Task<Edit.Command> EditAsync([FromRoute] Edit.Query query)
        {
            return _mediator.Send(query);
        }

        [HttpPost("edit")]
        public Task EditAsync(Edit.Command command)
        {
            return _mediator.Send(command);
        }
    }
}