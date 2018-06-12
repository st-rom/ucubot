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


namespace ucubot.DBRepository
{
    public class LessonSignalEndpointDBRepository : ILessonSignalRepository

    {
        private readonly IConfiguration _configuration;

		public LessonSignalEndpointDBRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		
		public IEnumerable<LessonSignalDto> ShowSignalsDBRep()
		{
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			using (connection){
				return connection.Query<LessonSignalDto>("SELECT lesson_signal.id as Id, time_stamp as Timestamp, signal_type as Type, student.user_id as UserId FROM lesson_signal INNER JOIN student ON student_id = student.id;").ToList();
			}
		}
		
	    
		public LessonSignalDto ShowSignalDBRep(long id)
		{
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			using (connection)
			{
				try{
					return connection.Query<LessonSignalDto>(
						"SELECT lesson_signal.id as Id, time_stamp as Timestamp, signal_type as Type, student.user_id as UserId FROM lesson_signal INNER JOIN student ON student_id = student.id WHERE lesson_signal.id = @id;",
						new {Id = id}).ToList()[0];
				}
				catch{
					return null;
				}
			}
		}
		
	    
		public int CreateSignalDBRep(SlackMessage message)
		{
			var userId = message.user_id;
			var signalType = message.text.ConvertSlackMessageToSignalType();
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			using (connection)
			{
				connection.Open();
				var t0 = connection.Query<LessonSignalDto>("SELECT * FROM student WHERE user_id = @userId;", new {UserId = userId}).ToList();	
				
				if(t0.Count == 0)
				{
					return 400;
				}
				var id = t0[0].Id;
				
				var command = 
					new MySqlCommand("INSERT INTO lesson_signal (student_id, signal_type, time_stamp) VALUES (@id, @signalType, @timeStamp);", connection);
				command.Parameters.AddWithValue("id", id);
				command.Parameters.AddWithValue("signalType", signalType);
				command.Parameters.AddWithValue("timeStamp", DateTime.Now);
				command.ExecuteNonQuery();
				connection.Close();
			}
			return 0;
		}
		
		public void RemoveSignalDBRep(long id)
		{
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			using (connection)
			{
				connection.Open();
				var command = new MySqlCommand("DELETE FROM lesson_signal WHERE id = @id;", connection);
				command.Parameters.AddWithValue("id", id);
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
	}
}