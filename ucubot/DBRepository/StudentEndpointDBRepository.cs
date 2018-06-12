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
    public class StudentEndpointDBRepository : IStudentRepository
    {
        private readonly IConfiguration _configuration;

        public StudentEndpointDBRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Student> ShowStudentsDBRep()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);
            using (connection){
                return connection.Query<Student>("SELECT id as Id, user_id as UserId, first_name as FirstName, last_name as LastName FROM student;").ToList();
            }
        }
        
        public Student ShowStudentDBRep(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);
            using (connection)
            {
                var studs = connection.Query<Student>("SELECT id as Id, user_id as UserId, first_name as FirstName, last_name as LastName FROM student WHERE id = @id;", new {Id = id}).ToList();
                if(studs.Count == 0){
                    return null; 
                }
                return studs[0];
            }
        }

        public int CreateSignalDBRep(Student stud)
        {
            var userId = stud.UserId;
            var firstName = stud.FirstName;
            var lastName = stud.LastName;
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                var t = connection
                    .Query<LessonSignalDto>("SELECT * FROM student WHERE user_id = @userId;", new {UserId = userId})
                    .ToList();
                if (t.Count > 0)
                {
                    return 409;
                }

                var command =
                    new MySqlCommand(
                        "INSERT INTO student (user_id, first_name, last_name) VALUES (@userId, @firstName, @lastName);",
                        connection);
                command.Parameters.AddWithValue("userId", userId);
                command.Parameters.AddWithValue("firstName", firstName);
                command.Parameters.AddWithValue("lastName", lastName);
                command.ExecuteNonQuery();
                connection.Close();
            }
            return 0;
        }


        public int UpdateStudentDBRep(Student stud)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);
            var id = stud.Id;
            var userId = stud.UserId;
            var firstName = stud.FirstName;
            var lastName = stud.LastName;
            using (connection)
            {
                connection.Open();
                try{
                    var command =
                        new MySqlCommand(
                            "UPDATE student SET user_id = @userId, first_name = @firstName, last_name = @lastName WHERE id = @id;",
                            connection);
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("firstName", firstName);
                    command.Parameters.AddWithValue("lastName", lastName);
                    command.Parameters.AddWithValue("id", id);
                    command.ExecuteNonQuery();
                }
                catch{
                    return 409;
                }

                connection.Close();
            }
            return 0;

        }
        
        public int DeleteStudentDBRep(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                try{
                    var command = new MySqlCommand("DELETE FROM student WHERE id = @id;", connection);
                    command.Parameters.AddWithValue("id", id);
                    command.ExecuteNonQuery();
                }
                catch{
                    return 409;
                }

                connection.Close();
            }
            return 0;
        }

    }
}