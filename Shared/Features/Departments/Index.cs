using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Domain.UniversityAggregate;
using ContosoUniversity.Infrastructure;
using DelegateDecompiler.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features.Departments
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Department> Departments { get; set; }

            public class Department
            {
                public string Name { get; set; }

                [DisplayFormat(DataFormatString = "{0:C0}")]
                public decimal Budget { get; set; }

                [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
                public DateTime StartDate { get; set; }

                public int Id { get; set; }

                public string AdministratorFullName { get; set; }
            }
        }


        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Department, Result.Department>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly SchoolContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext context,
                IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public async Task<Result> Handle(Query message, CancellationToken token)
            {
                var departments = await _context
                    .Departments
                    .ProjectTo<Result.Department>(_configuration)
                    .DecompileAsync()
                    .ToListAsync(token);

                return new Result { Departments = departments };
            }
        }
    }
}