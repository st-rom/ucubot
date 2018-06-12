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
    public class StudentSignalsEndpointController : Controller
    {

        private readonly IStudentSignalsRepository _studentSignalsRepository;
		
		
        public StudentSignalsEndpointController(IStudentSignalsRepository studentSignalsRepository)
        {
            _studentSignalsRepository = studentSignalsRepository;
        }

        [HttpGet]
        public IEnumerable<StudentSignal> ShowStudents()
        {
            return _studentSignalsRepository.ShowStudentSignalsDBRep();
        }
    }
}
