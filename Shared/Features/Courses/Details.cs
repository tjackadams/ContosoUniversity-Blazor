using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Features.Courses
{
    public class Details
    {
        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Course, Command>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly IConfigurationProvider _configuration;
            private readonly SchoolContext _db;

            public QueryHandler(SchoolContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public Task<Command> Handle(Query message, CancellationToken token)
            {
                return _db.Courses
                    .Where(c => c.Id == message.Id)
                    .ProjectTo<Command>(_configuration)
                    .SingleOrDefaultAsync(token);
            }
        }

        public class Command : IRequest
        {
            [Display(Name = "Number")]
            public int Id { get; set; }

            public string Title { get; set; }
            public int Credits { get; set; }

            [Display(Name = "Department")]
            public string DepartmentName { get; set; }
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
                var course = await _db.Courses.FindAsync(message.Id);

                _db.Courses.Remove(course);

                return default;
            }
        }
    }
}