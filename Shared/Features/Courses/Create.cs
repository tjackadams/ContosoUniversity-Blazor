using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContosoUniversity.Domain.UniversityAggregate;
using ContosoUniversity.Features.Courses.Validation;
using ContosoUniversity.Infrastructure;
using ContosoUniversity.Infrastructure.ModelBinding;
using FluentValidation;
using MediatR;

namespace ContosoUniversity.Features.Courses
{
   public  class Create
    {
        public class Command : IRequest<int>
        {
            [IgnoreMap]
            public int Number { get; set; }

            public string Title { get; set; }
            public int Credits { get; set; }

            [BindRequestProperty]
            public Department Department { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Course>(MemberList.Source);
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Number).NotEmpty();

                RuleFor(p => p.Title)
                    .SetValidator(new TitleValidator());

                RuleFor(p => (int?)p.Credits)
                    .SetValidator(new CreditsValidator());
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly SchoolContext _context;
            private readonly IMapper _mapper;

            public Handler(SchoolContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<int> Handle(Command message, CancellationToken token)
            {
                var course = _mapper.Map<Command, Course>(message);
                course.Id = message.Number;

                _context.Courses.Add(course);

                await _context.SaveChangesAsync(token);

                return course.Id;
            }
        }
    }
}
