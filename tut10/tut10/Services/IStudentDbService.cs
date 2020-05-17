using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using tut10.DTO_s;
using tut10.Entities;

namespace tut10.Models
{
    public interface IStudentDbService
    {
        public IEnumerable<GetStudentsResponse> GetStudents();

        public  void AddStudent(Student student);
        public void DeleteStudent(Student student);
        public void UpdateStudent(Student student);

        public Enrollment EnrollStudent(EnrollStudentRequest request);
        public Enrollment PromoteStudent(PromoteStudentRequest request);
    }
}
