﻿using System;
using System.Threading.Tasks;
using ContosoUniversity.Domain.UniversityAggregate;
using ContosoUniversity.Features.Instructors;
using Shouldly;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;
using Details = ContosoUniversity.Features.Courses.Details;

namespace ContosoUniversity.IntegrationTests.Features.Courses
{
    public class DetailsTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_query_for_details()
        {
            var adminId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George", LastName = "Costanza", HireDate = DateTime.Today
            });

            var dept = new Department
            {
                Name = "History", InstructorID = adminId, Budget = 123m, StartDate = DateTime.Today
            };

            var course = new Course { Credits = 4, Department = dept, Id = NextCourseNumber(), Title = "English 101" };

            await InsertAsync(dept, course);

            var result = await SendAsync(new Details.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.DepartmentName.ShouldBe(dept.Name);
            result.Title.ShouldBe(course.Title);
        }
    }
}