using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContosoUniversity.Domain.UniversityAggregate;
using ContosoUniversity.Infrastructure;
using ContosoUniversity.Infrastructure.AutoMapper;
using MediatR;

namespace ContosoUniversity.Features.Courses
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Course> Courses { get; set; }

            public class Course
            {
                public int Id { get; set; }
                public string Title { get; set; }
                public int Credits { get; set; }
                public string DepartmentName { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Course, Result.Course>();
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IConfigurationProvider _configuration;
            private readonly SchoolContext _db;

            public Handler(SchoolContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Result> Handle(Query message, CancellationToken token)
            {
                var courses = await _db.Courses
                    .OrderBy(d => d.Id)
                    .ProjectToListAsync<Result.Course>(_configuration);

                return new Result { Courses = courses };
            }
        }
    }
}