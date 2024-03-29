﻿using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Features.Departments;
using ContosoUniversity.Shared.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Index = ContosoUniversity.Shared.Features.Departments.Index;

namespace ContosoUniversity.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator, SchoolContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public Task<Department[]> GetAllAsync()
    {
        return _context.Departments.ToArrayAsync();
    }

    [HttpPost("create")]
    public Task CreateAsync(Create.Command command)
    {
        return _mediator.Send(command);
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
    public Task<Details.Model> DetailsAsync([FromRoute] Details.Query query)
    {
        return _mediator.Send(query);
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

    [HttpGet("index")]
    public Task<Index.Result> IndexAsync()
    {
        return _mediator.Send(new Index.Query());
    }
}