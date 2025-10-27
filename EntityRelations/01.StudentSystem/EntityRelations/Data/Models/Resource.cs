using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        //o ResourceId
        //o Name – up to 50 characters, unicode
        //o   Url – not unicode
        //o ResourceType – enum, can be Video, Presentation, Document or Other
        //o   CourseId

        public int ResourceId { get; set; }

        public string Name { get; set; } = null!;

        public string Url { get; set; } = null!;

        public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

    }

    public enum ResourceType
    {
        Video,
        Presentation,
        Document,
        Other
    }
}
