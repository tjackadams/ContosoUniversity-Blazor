using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContosoUniversity.Shared.Domain.SeedWork;
using DelegateDecompiler;

namespace ContosoUniversity.Shared.Domain.UniversityAggregate
{
    public abstract class Person : Entity
    {
        [Computed]
        [Display(Name = "Full Name")]
        public string FullName => LastName + ", " + FirstMidName;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }
    }
}