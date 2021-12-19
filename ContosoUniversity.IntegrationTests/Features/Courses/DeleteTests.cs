﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Features.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;
using Delete = ContosoUniversity.Shared.Features.Courses.Delete;


namespace ContosoUniversity.IntegrationTests.Features.Courses
{
    public class DeleteTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_delete()
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

            await SendAsync(new Delete.Command { Id = course.Id });

            var result =
                await ExecuteDbContextAsync(db => db.Courses.Where(c => c.Id == course.Id).SingleOrDefaultAsync());

            result.ShouldBeNull();
        }

        [Fact]
        public async Task Should_query_for_command()
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

            var result = await SendAsync(new Delete.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.DepartmentName.ShouldBe(dept.Name);
            result.Title.ShouldBe(course.Title);
        }
    }
}