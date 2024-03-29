﻿using ContosoUniversity.Shared.Features.Students;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Index = ContosoUniversity.Shared.Features.Students.Index;

namespace ContosoUniversity.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public Task CreateAsync(Create.Command command) { return _mediator.Send(command); }

    [HttpGet("{id}/delete")]
    public Task<Delete.Command> DeleteAsync([FromRoute] Delete.Query query) { return _mediator.Send(query); }

    [HttpPost("delete")]
    public Task DeleteAsync(Delete.Command command) { return _mediator.Send(command); }

    [HttpGet("{id}/details")]
    public Task<Details.Model> DetailsAsync([FromRoute] Details.Query query) { return _mediator.Send(query); }

    [HttpGet("{id}/edit")]
    public Task<Edit.Command> EditAsync([FromRoute] Edit.Query query) { return _mediator.Send(query); }

    [HttpPost("edit")]
    public Task EditAsync(Edit.Command command) { return _mediator.Send(command); }

    [HttpGet("index")]
    public Task<Index.Result> IndexAsync([FromQuery] Index.Query query)
    {
        return _mediator.Send(query);
    }
}