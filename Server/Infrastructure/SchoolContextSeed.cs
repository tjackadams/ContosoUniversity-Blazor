using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace ContosoUniversity.Server.Infrastructure
{
    public class SchoolContextSeed
    {
        public async Task SeedAsync(SchoolContext context, IWebHostEnvironment env,
            ILogger<SchoolContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(SchoolContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                // Look for any students.
                if (await context.Students.AnyAsync())
                {
                    return; // DB has been seeded
                }

                var students = new[]
                {
                    new Student
                    {
                        FirstMidName = "Carson",
                        LastName = "Alexander",
                        EnrollmentDate = DateTime.Parse("2010-09-01")
                    },
                    new Student
                    {
                        FirstMidName = "Meredith",
                        LastName = "Alonso",
                        EnrollmentDate = DateTime.Parse("2012-09-01")
                    },
                    new Student
                    {
                        FirstMidName = "Arturo",
                        LastName = "Anand",
                        EnrollmentDate = DateTime.Parse("2013-09-01")
                    },
                    new Student
                    {
                        FirstMidName = "Gytis",
                        LastName = "Barzdukas",
                        EnrollmentDate = DateTime.Parse("2012-09-01")
                    },
                    new Student
                    {
                        FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2012-09-01")
                    },
                    new Student
                    {
                        FirstMidName = "Peggy",
                        LastName = "Justice",
                        EnrollmentDate = DateTime.Parse("2011-09-01")
                    },
                    new Student
                    {
                        FirstMidName = "Laura",
                        LastName = "Norman",
                        EnrollmentDate = DateTime.Parse("2013-09-01")
                    },
                    new Student
                    {
                        FirstMidName = "Nino",
                        LastName = "Olivetto",
                        EnrollmentDate = DateTime.Parse("2005-09-01")
                    }
                };

                foreach (Student s in students)
                {
                    context.Students.Add(s);
                }

                await context.SaveChangesAsync();

                var instructors = new[]
                {
                    new Instructor
                    {
                        FirstMidName = "Kim", LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11")
                    },
                    new Instructor
                    {
                        FirstMidName = "Fadi", LastName = "Fakhouri", HireDate = DateTime.Parse("2002-07-06")
                    },
                    new Instructor
                    {
                        FirstMidName = "Roger", LastName = "Harui", HireDate = DateTime.Parse("1998-07-01")
                    },
                    new Instructor
                    {
                        FirstMidName = "Candace", LastName = "Kapoor", HireDate = DateTime.Parse("2001-01-15")
                    },
                    new Instructor
                    {
                        FirstMidName = "Roger", LastName = "Zheng", HireDate = DateTime.Parse("2004-02-12")
                    }
                };

                foreach (Instructor i in instructors)
                {
                    context.Instructors.Add(i);
                }

                await context.SaveChangesAsync();

                var departments = new[]
                {
                    new Department
                    {
                        Name = "English",
                        Budget = 350000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                    },
                    new Department
                    {
                        Name = "Mathematics",
                        Budget = 100000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID = instructors.Single(i => i.LastName == "Fakhouri").Id
                    },
                    new Department
                    {
                        Name = "Engineering",
                        Budget = 350000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                    },
                    new Department
                    {
                        Name = "Economics",
                        Budget = 100000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID = instructors.Single(i => i.LastName == "Kapoor").Id
                    }
                };

                foreach (Department d in departments)
                {
                    context.Departments.Add(d);
                }

                await context.SaveChangesAsync();

                var courses = new[]
                {
                    new Course
                    {
                        Id = 1050,
                        Title = "Chemistry",
                        Credits = 3,
                        DepartmentID = departments.Single(s => s.Name == "Engineering").Id
                    },
                    new Course
                    {
                        Id = 4022,
                        Title = "Microeconomics",
                        Credits = 3,
                        DepartmentID = departments.Single(s => s.Name == "Economics").Id
                    },
                    new Course
                    {
                        Id = 4041,
                        Title = "Macroeconomics",
                        Credits = 3,
                        DepartmentID = departments.Single(s => s.Name == "Economics").Id
                    },
                    new Course
                    {
                        Id = 1045,
                        Title = "Calculus",
                        Credits = 4,
                        DepartmentID = departments.Single(s => s.Name == "Mathematics").Id
                    },
                    new Course
                    {
                        Id = 3141,
                        Title = "Trigonometry",
                        Credits = 4,
                        DepartmentID = departments.Single(s => s.Name == "Mathematics").Id
                    },
                    new Course
                    {
                        Id = 2021,
                        Title = "Composition",
                        Credits = 3,
                        DepartmentID = departments.Single(s => s.Name == "English").Id
                    },
                    new Course
                    {
                        Id = 2042,
                        Title = "Literature",
                        Credits = 4,
                        DepartmentID = departments.Single(s => s.Name == "English").Id
                    }
                };

                foreach (Course c in courses)
                {
                    context.Courses.Add(c);
                }

                await context.SaveChangesAsync();

                var officeAssignments = new[]
                {
                    new OfficeAssignment
                    {
                        InstructorID = instructors.Single(i => i.LastName == "Fakhouri").Id,
                        Location = "Smith 17"
                    },
                    new OfficeAssignment
                    {
                        InstructorID = instructors.Single(i => i.LastName == "Harui").Id, Location = "Gowan 27"
                    },
                    new OfficeAssignment
                    {
                        InstructorID = instructors.Single(i => i.LastName == "Kapoor").Id,
                        Location = "Thompson 304"
                    }
                };

                foreach (OfficeAssignment o in officeAssignments)
                {
                    context.OfficeAssignments.Add(o);
                }

                await context.SaveChangesAsync();

                var courseInstructors = new[]
                {
                    new CourseAssignment
                    {
                        CourseID = courses.Single(c => c.Title == "Chemistry").Id,
                        InstructorID = instructors.Single(i => i.LastName == "Kapoor").Id
                    },
                    new CourseAssignment
                    {
                        CourseID = courses.Single(c => c.Title == "Chemistry").Id,
                        InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                    },
                    new CourseAssignment
                    {
                        CourseID = courses.Single(c => c.Title == "Microeconomics").Id,
                        InstructorID = instructors.Single(i => i.LastName == "Zheng").Id
                    },
                    new CourseAssignment
                    {
                        CourseID = courses.Single(c => c.Title == "Macroeconomics").Id,
                        InstructorID = instructors.Single(i => i.LastName == "Zheng").Id
                    },
                    new CourseAssignment
                    {
                        CourseID = courses.Single(c => c.Title == "Calculus").Id,
                        InstructorID = instructors.Single(i => i.LastName == "Fakhouri").Id
                    },
                    new CourseAssignment
                    {
                        CourseID = courses.Single(c => c.Title == "Trigonometry").Id,
                        InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                    },
                    new CourseAssignment
                    {
                        CourseID = courses.Single(c => c.Title == "Composition").Id,
                        InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                    },
                    new CourseAssignment
                    {
                        CourseID = courses.Single(c => c.Title == "Literature").Id,
                        InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                    }
                };

                foreach (CourseAssignment ci in courseInstructors)
                {
                    context.CourseAssignments.Add(ci);
                }

                await context.SaveChangesAsync();

                var enrollments = new[]
                {
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Alexander").Id,
                        CourseID = courses.Single(c => c.Title == "Chemistry").Id,
                        Grade = Grade.A
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Alexander").Id,
                        CourseID = courses.Single(c => c.Title == "Microeconomics").Id,
                        Grade = Grade.C
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Alexander").Id,
                        CourseID = courses.Single(c => c.Title == "Macroeconomics").Id,
                        Grade = Grade.B
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Alonso").Id,
                        CourseID = courses.Single(c => c.Title == "Calculus").Id,
                        Grade = Grade.B
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Alonso").Id,
                        CourseID = courses.Single(c => c.Title == "Trigonometry").Id,
                        Grade = Grade.B
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Alonso").Id,
                        CourseID = courses.Single(c => c.Title == "Composition").Id,
                        Grade = Grade.B
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Anand").Id,
                        CourseID = courses.Single(c => c.Title == "Chemistry").Id
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Anand").Id,
                        CourseID = courses.Single(c => c.Title == "Microeconomics").Id,
                        Grade = Grade.B
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Barzdukas").Id,
                        CourseID = courses.Single(c => c.Title == "Chemistry").Id,
                        Grade = Grade.B
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Li").Id,
                        CourseID = courses.Single(c => c.Title == "Composition").Id,
                        Grade = Grade.B
                    },
                    new Enrollment
                    {
                        StudentID = students.Single(s => s.LastName == "Justice").Id,
                        CourseID = courses.Single(c => c.Title == "Literature").Id,
                        Grade = Grade.B
                    }
                };

                foreach (Enrollment e in enrollments)
                {
                    var enrollmentInDataBase = context.Enrollments.SingleOrDefault(s => s.Student.Id == e.StudentID &&
                                                                                        s.Course.Id == e.Id);
                    if (enrollmentInDataBase == null)
                    {
                        context.Enrollments.Add(e);
                    }
                }

                await context.SaveChangesAsync();
            });
        }

        private static AsyncRetryPolicy CreatePolicy(ILogger<SchoolContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(
                retries,
                retry => TimeSpan.FromSeconds(5),
                (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(exception,
                        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        prefix, exception.GetType().Name, exception.Message, retry, retries);
                }
            );
        }
    }
}