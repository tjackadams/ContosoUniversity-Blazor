using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using DelegateDecompiler.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Features.Departments
{
    public class Details
    {
        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public string Name { get; set; }

            public decimal Budget { get; set; }

            public DateTime StartDate { get; set; }

            public int Id { get; set; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Department, Model>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Model>
        {
            private readonly SchoolContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext context, IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public Task<Model> Handle(Query message,
                CancellationToken token)
            {
                return _context.Departments
                    .Where(m => m.Id == message.Id)
                    .ProjectTo<Model>(_configuration)
                    .DecompileAsync()
                    .SingleOrDefaultAsync(token);
            }
        }
    }
}