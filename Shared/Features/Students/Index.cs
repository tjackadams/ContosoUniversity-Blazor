using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using ContosoUniversity.Shared.Infrastructure.AutoMapper;
using ContosoUniversity.Shared.Infrastructure.Collections;
using MediatR;

namespace ContosoUniversity.Shared.Features.Students
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
            public string? SortOrder { get; set; }
            public string? CurrentFilter { get; set; }
            public string? SearchString { get; set; }
            public int? Page { get; set; }
        }

        public class Result
        {
            public string CurrentSort { get; set; }
            public string NameSortParm { get; set; }
            public string DateSortParm { get; set; }
            public string CurrentFilter { get; set; }
            public string SearchString { get; set; }

            public PaginatedList<Model> Results { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }

            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            public string LastName { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public int EnrollmentsCount { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Student, Model>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly SchoolContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext context, IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public async Task<Result> Handle(Query message, CancellationToken token)
            {
                var model = new Result
                {
                    CurrentSort = message.SortOrder,
                    NameSortParm = string.IsNullOrEmpty(message.SortOrder) ? "name_desc" : "",
                    DateSortParm = message.SortOrder == "Date" ? "date_desc" : "Date"
                };

                if (message.SearchString != null)
                {
                    message.Page = 1;
                }
                else
                {
                    message.SearchString = message.CurrentFilter;
                }

                model.CurrentFilter = message.SearchString;
                model.SearchString = message.SearchString;

                IQueryable<Student> students = _context.Students;
                if (!string.IsNullOrEmpty(message.SearchString))
                {
                    students = students.Where(s => s.LastName.Contains(message.SearchString)
                                                   || s.FirstMidName.Contains(message.SearchString));
                }

                switch (message.SortOrder)
                {
                    case "name_desc":
                        students = students.OrderByDescending(s => s.LastName);
                        break;
                    case "Date":
                        students = students.OrderBy(s => s.EnrollmentDate);
                        break;
                    case "date_desc":
                        students = students.OrderByDescending(s => s.EnrollmentDate);
                        break;
                    default: // Name ascending 
                        students = students.OrderBy(s => s.LastName);
                        break;
                }

                const int pageSize = 3;
                var pageNumber = message.Page ?? 1;
                model.Results = await students
                    .ProjectTo<Model>(_configuration)
                    .PaginatedListAsync(pageNumber, pageSize);

                return model;
            }
        }
    }
}