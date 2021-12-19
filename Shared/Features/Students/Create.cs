using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using FluentValidation;
using MediatR;

namespace ContosoUniversity.Shared.Features.Students
{
    public class Create
    {
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Student>(MemberList.Source);
            }
        }

        public class Command : IRequest<int>
        {
            public string LastName { get; set; }

            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            public DateTime? EnrollmentDate { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.LastName).NotNull().Length(1, 50);
                RuleFor(m => m.FirstMidName).NotNull().Length(1, 50);
                RuleFor(m => m.EnrollmentDate).NotNull();
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
                var student = _mapper.Map<Command, Student>(message);

                _context.Students.Add(student);

                await _context.SaveChangesAsync(token);

                return student.Id;
            }
        }
    }
}