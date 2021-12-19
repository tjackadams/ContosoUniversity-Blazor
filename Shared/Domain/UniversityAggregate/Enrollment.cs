using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContosoUniversity.Shared.Domain.SeedWork;

namespace ContosoUniversity.Shared.Domain.UniversityAggregate
{
    public enum Grade
    {
        A,
        B,
        C,
        D,
        F
    }

    public class Enrollment : Entity
    {
        [Column("EnrollmentID")]
        public override int Id { get; set; }

        public int CourseID { get; set; }
        public int StudentID { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}