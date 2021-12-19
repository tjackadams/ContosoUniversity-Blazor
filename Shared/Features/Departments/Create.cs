using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Features.Departments.Validation;
using ContosoUniversity.Shared.Infrastructure;
using ContosoUniversity.Shared.Infrastructure.ModelBinding;
using FluentValidation;
using MediatR;

namespace ContosoUniversity.Shared.Features.Departments
{
    public class Create
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name)
                    .SetValidator(new NameValidator());
                RuleFor(m => m.Budget).NotNull();
                RuleFor(m => m.StartDate).NotNull();
                RuleFor(m => m.Administrator).NotNull();
            }
        }

        public class MappingProfiler : Profile
        {
            public MappingProfiler()
            {
                CreateMap<Command, Department>(MemberList.Source);
            }
        }

        public class Command : IRequest<int>
        {
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; set; }

            [DataType(DataType.Currency)]
            [Column(TypeName = "money")]
            public decimal? Budget { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? StartDate { get; set; }

            [BindRequestProperty]
            public Instructor Administrator { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly SchoolContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(SchoolContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<int> Handle(Command message, CancellationToken token)
            {
                var department = _mapper.Map<Command, Department>(message);

                _context.Departments.Add(department);

                await _context.SaveChangesAsync(token);

                return department.Id;
            }
        }
    }
}