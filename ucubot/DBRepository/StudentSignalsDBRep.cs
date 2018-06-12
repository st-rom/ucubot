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
    public class StudentSignalsDBRepository : IStudentSignalsRepository
    {
        private readonly IConfiguration _configuration;

        public StudentSignalsDBRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<StudentSignal> ShowStudentSignalsDBRep()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var connection = new MySqlConnection(connectionString);
            using (connection){
                return connection.Query<StudentSignal>("SELECT * FROM student_signals;").ToList();
            }
        }

    }
}