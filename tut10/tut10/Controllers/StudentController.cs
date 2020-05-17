using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using tut10.DTO_s;
using tut10.Entities;
using tut10.Models;

namespace tut10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {


        private readonly IStudentDbService _dbService;
        public StudentController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _dbService.GetStudents();
            return Ok(students);
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            _dbService.AddStudent(student);
            return Ok("the student was inserted");
        }

        [HttpPut]
        public IActionResult UpdateStudent(Student student)
        {
            _dbService.UpdateStudent(student);
            return Ok("the student was updated");
        }

        [HttpDelete]
        public IActionResult DeleteStudent(Student student)
        {
            _dbService.DeleteStudent(student);
            return Ok("the student was deleted");
        }

        [HttpPost]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            _dbService.PromoteStudent(request);
            return Ok("All students have been promoted");
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _dbService.EnrollStudent(request);
            return Ok("the student has been enrolled");
        }
    }
}