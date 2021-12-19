using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Features.Courses.Validation;
using ContosoUniversity.Shared.Infrastructure;
using ContosoUniversity.Shared.Infrastructure.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Features.Courses
{
    public class Edit
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

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly IConfigurationProvider _configuration;
            private readonly SchoolContext _context;

            public QueryHandler(SchoolContext context, IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public Task<Command> Handle(Query message, CancellationToken token)
            {
                return _context.Courses
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
            public int? Credits { get; set; }

            [BindRequestProperty]
            [Display(Name = "Department")]
            public Department Department { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Course, Command>().ReverseMap();
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Title)
                    .SetValidator(new TitleValidator());
                RuleFor(m => m.Credits)
                    .SetValidator(new CreditsValidator());
            }
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly SchoolContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(SchoolContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var course = await _context.Courses.FindAsync(request.Id);

                _mapper.Map(request, course);

                return default;
            }
        }
    }
}