using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Features.Departments
{
    public class Delete
    {
        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public string Name { get; set; }

            public decimal Budget { get; set; }

            public DateTime StartDate { get; set; }

            public int Id { get; set; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; set; }

            public byte[] RowVersion { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Department, Command>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly SchoolContext _db;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Command> Handle(Query message, CancellationToken token)
            {
                return await _db
                    .Departments
                    .Where(d => d.Id == message.Id)
                    .ProjectTo<Command>(_configuration)
                    .SingleOrDefaultAsync(token);
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly SchoolContext _context;

            public CommandHandler(SchoolContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command message, CancellationToken token)
            {
                var department = await _context.Departments.FindAsync(message.Id);

                _context.Departments.Remove(department);

                return default;
            }
        }
    }
}