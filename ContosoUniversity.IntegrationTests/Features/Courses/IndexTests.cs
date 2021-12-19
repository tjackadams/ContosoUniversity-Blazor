using System;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Features.Instructors;
using Shouldly;
using Xunit;
using Index = ContosoUniversity.Shared.Features.Courses.Index;

namespace ContosoUniversity.IntegrationTests.Features.Courses
{
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_return_all_courses()
        {
            var adminId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George", LastName = "Costanza", HireDate = DateTime.Today
            });

            var englishDept = new Department
            {
                Name = "English", InstructorID = adminId, Budget = 123m, StartDate = DateTime.Today
            };
            var historyDept = new Department
            {
                Name = "History", InstructorID = adminId, Budget = 123m, StartDate = DateTime.Today
            };

            var english = new Course
            {
                Credits = 4, Department = englishDept, Id = NextCourseNumber(), Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4, Department = historyDept, Id = NextCourseNumber(), Title = "History 101"
            };
            await InsertAsync(englishDept, historyDept, english, history);

            var result = await SendAsync(new Index.Query());

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBeGreaterThanOrEqualTo(2);
        }
    }
}