using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using n01569183Cumulative1.Models;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace n01569183Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();


        /// <summary>
        /// Generates a list of all teachers in the Database
        /// </summary>
        /// <example>
        /// GET: api/Teacherdata/ListTeachers -> List of all Teachers
        /// </example>
        /// <returns>List of type Teacher</returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchParam?}/{SalaryParam?}/{HireParam?}")]

        public IEnumerable<Teacher> ListTeachers(string SearchParam = null, decimal SalaryParam = -1, string HireParam = null )
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Teachers";
            if (SearchParam != null)
            {
                query += " WHERE lower(CONCAT(teacherfname, ' ', teacherlname)) LIKE @search";
                if (SalaryParam > -1) query += " AND salary >= @salary";
                if (HireParam != null) query += " AND hiredate >= @hiredate";
            }
            MySqlCommand cmd = Conn.CreateCommand();
            
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@search", "%" + SearchParam + "%");
            cmd.Parameters.AddWithValue("@salary", SalaryParam);
            cmd.Parameters.AddWithValue("@hiredate", HireParam);
            cmd.Prepare();
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
            Conn.Clone();
            return Teachers;
        }

        /// <summary>
        /// Select a Teacher by their ID in the Database
        /// </summary>
        /// <param name="id">Integer ID of the Teacher</param>
        /// <example>
        /// GET: api/TeacherData/SelectTeacher/5 -> Teacher at ID of 5
        /// </example>
        /// <returns>Teacher object</returns>
        [HttpGet]
        [Route("api/TeacherData/SelectTeacher/{id}")]
        public Teacher SelectTeacher(int id)
        {

            string query = "SELECT * FROM Teachers INNER JOIN classes ON classes.teacherid = teachers.teacherid  WHERE teachers.teacherid = @id ";

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            Teacher SelectedTeacher = new Teacher();
            List<Class> teacherClasses = new List<Class>();

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

                Class _class = new Class()
                {
                    ClassId = Convert.ToInt32(ResultSet["classid"]),
                    ClassCode = ResultSet["classcode"].ToString(),
                    TeacherId = Convert.ToInt32(ResultSet["teacherid"]),
                    StartDate = Convert.ToDateTime(ResultSet["startdate"]),
                    FinishDate = Convert.ToDateTime(ResultSet["finishdate"]),
                    ClassName = ResultSet["classname"].ToString()
                };

                SelectedTeacher = _teacher;
                teacherClasses.Add(_class);
            }
            SelectedTeacher.classList = teacherClasses;
            Debug.WriteLine(SelectedTeacher.classList.Count);
            return SelectedTeacher;

        }


    }
}
