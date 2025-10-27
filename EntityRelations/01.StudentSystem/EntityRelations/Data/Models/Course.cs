using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
            //        o CourseId
            //o Name – up to 80 characters, unicode
            //o   Description – unicode, not required
            //o StartDate
            //o EndDate
            //o Price

        public int CourseId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

        public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; } = new List<StudentCourse>();

    }
}
