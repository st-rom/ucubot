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
using Dapper;


namespace ucubot.DBRepository{
    public interface IStudentRepository
    {
        IEnumerable<Student> ShowStudentsDBRep();
        Student ShowStudentDBRep(long id);
        int CreateSignalDBRep(Student student);
        int UpdateStudentDBRep(Student stud);
        int DeleteStudentDBRep(long id);
    }
}