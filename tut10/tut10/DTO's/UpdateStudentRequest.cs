using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut10.DTO_s
{
    public class UpdateStudentRequest
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }
    }
}
