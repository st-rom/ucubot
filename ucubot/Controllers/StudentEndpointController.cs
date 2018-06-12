using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using ucubot.DBRepository;


namespace ucubot.Controllers
{
	[Route("api/[controller]")]
	public class StudentEndpointController : Controller
	{

		private readonly IStudentRepository _studentRepository;
		
		
		public StudentEndpointController(IStudentRepository studentRepository)
		{
			_studentRepository = studentRepository;
		}

		[HttpGet]
		public IEnumerable<Student> ShowStudents()
		{

			return _studentRepository.ShowStudentsDBRep();


			//StudentRepository studentRepository = new ();

		}
		
		[HttpGet("{id}")]
		public Student ShowStudent(long id)
		{
			return _studentRepository.ShowStudentDBRep(id);
		}
		
		
		[HttpPost]
		public async Task<IActionResult> CreateStudent(Student student)
		{
			var sig = _studentRepository.CreateSignalDBRep(student);
			if (sig == 409)
			{
				return StatusCode(409);
			}

			return Accepted();

		}


		[HttpPut]
		public async Task<IActionResult> UpdateStudent(Student stud)
		{
			var sig = _studentRepository.UpdateStudentDBRep(stud);
			if (sig == 409)
			{
				return StatusCode(409);
			}

			return Accepted();
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStudent(long id)
		{
			var sig = _studentRepository.DeleteStudentDBRep(id);
			if (sig == 409)
			{
				return StatusCode(409);
			}

			return Accepted();
		}
	}
}
