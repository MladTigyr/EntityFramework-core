using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        //o HomeworkId
        //o Content – string, linking to a file, not unicode
        //o   ContentType - enum, can be Application, Pdf or Zip
        //o   SubmissionTime
        //o   StudentId
        //o   CourseId

        public int HomeworkId { get; set; }

        public string Content { get; set; } = null!;

        public ContentType ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;
    }

    public enum ContentType
    {
        Application,
        Pdf,
        Zip
    }
}
