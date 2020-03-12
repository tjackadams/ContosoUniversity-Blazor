using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ContosoUniversity.Features.Instructors;

namespace ContosoUniversity.Domain.UniversityAggregate
{
    public class Instructor : Person
    {
        public Instructor()
        {
            CourseAssignments = new HashSet<CourseAssignment>();
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }

        public void Handle(Delete.Command _)
        {
            OfficeAssignment = null;
        }

        public void Handle(CreateEdit.Command message,
            IEnumerable<Course> courses)
        {
            UpdateDetails(message);

            UpdateInstructorCourses(message.AssignedCourses.Where(c => c.Assigned).ToList(), courses);
        }

        private void UpdateDetails(CreateEdit.Command message)
        {
            FirstMidName = message.FirstMidName;
            LastName = message.LastName;
            HireDate = message.HireDate.GetValueOrDefault();

            if (string.IsNullOrWhiteSpace(message.OfficeAssignmentLocation))
            {
                OfficeAssignment = null;
            }
            else if (OfficeAssignment == null)
            {
                OfficeAssignment = new OfficeAssignment { Location = message.OfficeAssignmentLocation };
            }
            else
            {
                OfficeAssignment.Location = message.OfficeAssignmentLocation;
            }
        }

        private void UpdateInstructorCourses(List<CreateEdit.Command.AssignedCourseData> selectedCourses, IEnumerable<Course> courses)
        {
            if (selectedCourses == null)
            {
                CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHs = new HashSet<int>(selectedCourses.Select(c => c.CourseId));
            var instructorCourses = new HashSet<int>
                (CourseAssignments.Select(c => c.CourseID));

            foreach (var course in courses)
            {
                if (selectedCoursesHs.Contains(course.Id))
                {
                    if (!instructorCourses.Contains(course.Id))
                    {
                        CourseAssignments.Add(new CourseAssignment { Course = course, Instructor = this });
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.Id))
                    {
                        var toRemove = CourseAssignments.Single(ci => ci.CourseID == course.Id);
                        CourseAssignments.Remove(toRemove);
                    }
                }
            }
        }
    }
}