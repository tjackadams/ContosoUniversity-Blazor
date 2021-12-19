using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Features.Instructors;
using Shouldly;
using Xunit;
using Index = ContosoUniversity.Shared.Features.Departments.Index;

namespace ContosoUniversity.IntegrationTests.Features.Departments
{
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_list_departments()
        {
            var adminId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George", LastName = "Costanza", HireDate = DateTime.Today
            });

            var dept = new Department
            {
                Name = "History", InstructorID = adminId, Budget = 123m, StartDate = DateTime.Today
            };
            var dept2 = new Department
            {
                Name = "English", InstructorID = adminId, Budget = 456m, StartDate = DateTime.Today
            };

            await InsertAsync(dept, dept2);

            var query = new Index.Query();

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.Departments.Count.ShouldBeGreaterThanOrEqualTo(2);
            result.Departments.Select(m => m.Id).ShouldContain(dept.Id);
            result.Departments.Select(m => m.Id).ShouldContain(dept2.Id);
        }
    }
}