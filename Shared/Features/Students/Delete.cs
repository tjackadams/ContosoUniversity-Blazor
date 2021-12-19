using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Features.Students
{
    public class Delete
    {
        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public int Id { get; set; }

            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            public string LastName { get; set; }
            public DateTime EnrollmentDate { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Student, Command>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly SchoolContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext context, IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public async Task<Command> Handle(Query message, CancellationToken token)
            {
                return await _context
                    .Students
                    .Where(s => s.Id == message.Id)
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
                _context.Students.Remove(await _context.Students.FindAsync(message.Id));

                return default;
            }
        }
    }
}