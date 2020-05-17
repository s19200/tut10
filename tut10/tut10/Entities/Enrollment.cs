using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut10.Entities
{
    public class Enrollment
    {
        public Enrollment()
        {
            Students = new HashSet<Student>();
        }
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }

        public virtual Studies IdStudyNavigation { get; set; }
        public virtual ICollection<Student> Students{ get; set; }
    }
}
