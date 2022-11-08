using MySql.Data.MySqlClient;
using n01569183Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace n01569183Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext Blog = new SchoolDbContext();

        [HttpGet]
        public IEnumerable<Teacher> ListTeachers()
        {
            MySqlConnection Conn = Blog.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "Select * from Teachers";
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Teacher> Teachers = new List<Teacher>();

            while (ResultSet.Read())
            {
                Teacher _teacher = new Teacher()
                {
                    Id = Convert.ToInt32(ResultSet["teacherid"]),
                    FName = ResultSet["teacherfname"].ToString(),
                    LName = ResultSet["teacherlname"].ToString(),
                    EmployeeNumber = ResultSet["employeenumber"].ToString(),
                    HireDate = Convert.ToDateTime(ResultSet["hiredate"]),
                    Salary = Convert.ToDecimal(ResultSet["salary"])
                };
                    
                Teachers.Add(_teacher);
            }

            Conn.Close();
            return Teachers;
        }

        [HttpGet]
        [Route("api/TeacherData/SelectTeacher/{id}")]
        public Teacher SelectTeacher(int id)
        {
            MySqlConnection Conn = Blog.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "Select * from Teachers WHERE teacherid=" + id;
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            Teacher SelectedTeacher = new Teacher();

            while (ResultSet.Read())
            {
                Teacher _teacher = new Teacher()
                {
                    Id = Convert.ToInt32(ResultSet["teacherid"]),
                    FName = ResultSet["teacherfname"].ToString(),
                    LName = ResultSet["teacherlname"].ToString(),
                    EmployeeNumber = ResultSet["employeenumber"].ToString(),
                    HireDate = Convert.ToDateTime(ResultSet["hiredate"]),
                    Salary = Convert.ToDecimal(ResultSet["salary"])
                };

                SelectedTeacher = _teacher;
            }
            return SelectedTeacher;

        }


    }
}
