﻿using System;
using System.Threading.Tasks;
using ContosoUniversity.Domain.UniversityAggregate;
using ContosoUniversity.Features.Students;
using Shouldly;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;

namespace ContosoUniversity.IntegrationTests.Features.Students
{
    public class DeleteTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_delete_student()
        {
            var createCommand = new Create.Command
            {
                FirstMidName = "Joe", LastName = "Schmoe", EnrollmentDate = DateTime.Today
            };

            var studentId = await SendAsync(createCommand);

            var deleteCommand = new Delete.Command { Id = studentId };

            await SendAsync(deleteCommand);

            var student = await FindAsync<Student>(studentId);

            student.ShouldBeNull();
        }

        [Fact]
        public async Task Should_get_delete_details()
        {
            var cmd = new Create.Command { FirstMidName = "Joe", LastName = "Schmoe", EnrollmentDate = DateTime.Today };

            var studentId = await SendAsync(cmd);

            var query = new Delete.Query { Id = studentId };

            var result = await SendAsync(query);

            result.FirstMidName.ShouldBe(cmd.FirstMidName);
            result.LastName.ShouldBe(cmd.LastName);
            result.EnrollmentDate.ShouldBe(cmd.EnrollmentDate.GetValueOrDefault());
        }
    }
}