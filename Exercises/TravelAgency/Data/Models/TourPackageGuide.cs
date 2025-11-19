using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Data.Models
{
    public class TourPackageGuide
    {
        public int TourPackageId { get; set; }

        public virtual TourPackage TourPackage { get; set; } = null!;

        public int GuideId { get; set; }

        public virtual Guide Guide { get; set; } = null!; 
    }
}
