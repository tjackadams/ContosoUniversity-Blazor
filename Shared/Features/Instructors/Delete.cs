using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Features.Instructors
{
    public class Delete
    {
        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class Command : IRequest
        {
            public int? Id { get; set; }

            public string LastName { get; set; }
            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime? HireDate { get; set; }

            [Display(Name = "Location")]
            public string OfficeAssignmentLocation { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Instructor, Command>();
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

            public Task<Command> Handle(Query message, CancellationToken token)
            {
                return _db
                    .Instructors
                    .Where(i => i.Id == message.Id)
                    .ProjectTo<Command>(_configuration)
                    .SingleOrDefaultAsync(token);
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public CommandHandler(SchoolContext db)
            {
                _db = db;
            }

            public async Task<Unit> Handle(Command message, CancellationToken token)
            {
                Instructor instructor = await _db.Instructors
                    .Include(i => i.OfficeAssignment)
                    .Where(i => i.Id == message.Id)
                    .SingleAsync(token);

                instructor.Handle(message);

                _db.Instructors.Remove(instructor);

                var department = await _db.Departments
                    .Where(d => d.InstructorID == message.Id)
                    .SingleOrDefaultAsync(token);
                if (department != null)
                {
                    department.InstructorID = null;
                }

                return default;
            }
        }
    }
}