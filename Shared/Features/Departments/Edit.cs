using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using ContosoUniversity.Shared.Infrastructure.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Features.Departments
{
    public class Edit
    {
        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
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

            public int Id { get; set; }
            public byte[] RowVersion { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotNull().Length(3, 50);
                RuleFor(m => m.Budget).NotNull();
                RuleFor(m => m.StartDate).NotNull();
                RuleFor(m => m.Administrator).NotNull();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Department, Command>().ReverseMap();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly IConfigurationProvider _configuration;
            private readonly SchoolContext _db;

            public QueryHandler(SchoolContext db,
                IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Command> Handle(Query message,
                CancellationToken token)
            {
                return await _db
                    .Departments
                    .Where(d => d.Id == message.Id)
                    .Include(d => d.Administrator)
                    .ProjectTo<Command>(_configuration)
                    .SingleOrDefaultAsync(token);
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly SchoolContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(SchoolContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request,
                CancellationToken token)
            {
                var department = await _context.Departments.FindAsync(request.Id);

                _mapper.Map(request, department);

                return default;
            }
        }
    }
}