using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;
using  ucubot.DBRepository;

namespace ucubot.Controllers
{
	[Route("api/[controller]")]
	public class LessonSignalEndpointController : Controller
	{
		private readonly ILessonSignalRepository _lessonSignalRepository;
		
		
		public LessonSignalEndpointController(ILessonSignalRepository lessonSignalRepository)
		{
			_lessonSignalRepository = lessonSignalRepository;
		}

		[HttpGet]
		public IEnumerable<LessonSignalDto> ShowSignals()
		{
			return _lessonSignalRepository.ShowSignalsDBRep();
		}
		
		[HttpGet("{id}")]
		public LessonSignalDto ShowSignal(long id)
		{
			return _lessonSignalRepository.ShowSignalDBRep(id);
		}
		
		[HttpPost]
		public async Task<IActionResult> CreateSignal(SlackMessage message)
		{
			var sig = _lessonSignalRepository.CreateSignalDBRep(message);
			if (sig == 400)
			{
				return BadRequest();
			}
			if (sig == 409)
			{
				return StatusCode(409);
			}
			return Accepted();
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> RemoveSignal(long id)
		{
			_lessonSignalRepository.RemoveSignalDBRep(id);
			return Accepted();
		}
	}
} 