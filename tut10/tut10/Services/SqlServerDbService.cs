using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut10.DTO_s;
using tut10.Entities;
using tut10.Models;

namespace tut10.Services
{
    public class SqlServerDbService : IStudentDbService
    {
        private readonly StudentContext _studentContext;

        public SqlServerDbService(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }
        public void AddStudent(Student student)
        {
            var newStudent = new Student
            {
                IndexNumber = student.IndexNumber,
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                IdEnrollment = student.IdEnrollment
            };

            _studentContext.Student.Add(newStudent);
        }

        public void DeleteStudent(Student student)
        {
            _studentContext.Student.Remove(student);
        }

        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {
            int generateEnrollment = 0;
            Enrollment enrollment = null;
            if((_studentContext.Studies.Any(s => s.IdStudy == request.Studies)) && (!_studentContext.Student.Any(s => s.IndexNumber == request.IndexNumber)  ))         //if there is such study and no such student
            {
                    generateEnrollment = _studentContext.Enrollment.Count() + 1;
                    enrollment = new Enrollment
                    {
                        IdEnrollment = generateEnrollment,
                        Semester = 1,
                        IdStudy = request.Studies,
                        StartDate = DateTime.Today
                    };

                    var newStudent = new Student
                    {
                        IndexNumber = request.IndexNumber,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        BirthDate = request.BirthDate,
                        IdEnrollment = generateEnrollment
                    };

                    AddStudent(newStudent);
            }
            return enrollment;
        } 

        public IEnumerable<GetStudentsResponse> GetStudents()
        {
            var students = _studentContext.Student
                .Include(s => s.IdEnrollmentNavigation)
                .ThenInclude(s => s.IdStudyNavigation)
                .Select(st => new GetStudentsResponse
                {
                    IndexNumber = st.IndexNumber,
                    FirstName = st.FirstName,
                    LastName = st.LastName,
                    BirthDate = st.BirthDate.ToShortDateString(),
                    Semester = st.IdEnrollmentNavigation.Semester,
                    Studies = st.IdEnrollmentNavigation.IdStudyNavigation.Name
                }).ToList();

            return students;
        }

        public Enrollment PromoteStudent(PromoteStudentRequest request)
        {
            Enrollment promoted = null; ;
            if (_studentContext.Studies.Any(s => s.IdStudy == request.Study))       //if such study exists
            {
                var anyEnrollment = _studentContext.Enrollment.Include(o => o.IdStudyNavigation)
                    .Where(o => o.Semester == request.Semester && o.IdStudyNavigation.IdStudy == request.Study)
                    .Any();

                var ifAlreadyPromoted = _studentContext.Enrollment.Include(o => o.IdStudyNavigation)
                    .Where(o => (o.Semester == request.Semester + 1 && o.IdStudyNavigation.IdStudy == request.Study))
                    .Any();

                if(anyEnrollment && !ifAlreadyPromoted)
                {
                    promoted = new Enrollment
                    {
                        IdEnrollment = _studentContext.Enrollment.Count() + 1,
                        Semester = request.Semester + 1,
                        IdStudy = request.Study,
                        StartDate = DateTime.Today
                    };
                    _studentContext.Enrollment.Add(promoted);

                    var studentsToPromote = _studentContext.Student.Include(o => o.IdEnrollmentNavigation)
                        .ThenInclude(o => o.IdStudyNavigation)
                        .Where(o => o.IdEnrollmentNavigation.Semester == request.Semester && o.IdEnrollmentNavigation.IdStudyNavigation.IdStudy == request.Study)
                        .ToList();

                    for(int i = 0; i<studentsToPromote.Count; i++)
                    {
                        studentsToPromote[i].IdEnrollment = promoted.IdEnrollment;
                        _studentContext.Entry(studentsToPromote[i]).Property("IdEnrollment").IsModified = true;
                    }
                }
            }
            return promoted;
        }

        public void UpdateStudent(Student student)
        {
            var updateStudent = new Student
            {
                IndexNumber = student.IndexNumber,
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                IdEnrollment = student.IdEnrollment
            };

            _studentContext.Student.Attach(updateStudent);
            _studentContext.Entry(updateStudent).State = EntityState.Modified;
            _studentContext.SaveChanges();

        }
    }
}


 


