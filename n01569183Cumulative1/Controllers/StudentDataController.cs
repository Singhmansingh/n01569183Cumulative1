using Microsoft.Ajax.Utilities;
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
    public class StudentDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Gets a list of all Students
        /// </summary>
        /// <returns>List of type Student</returns>
        [HttpGet]
        [Route("api/StudentData/ListStudents/{SearchParam?}")]
        public IEnumerable<Student> ListStudents(string SearchParam = null)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Students";
            if (SearchParam != null) query = "SELECT * FROM Students WHERE lower(Concat(studentfname,' ', studentlname)) LIKE @search";
            
            
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@search", "%" + SearchParam + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Student> Students = new List<Student>();

            while (ResultSet.Read())
            {
                Student _Student = new Student()
                {
                    StudentId = Convert.ToInt32(ResultSet["studentid"]),
                    StudentFName = ResultSet["studentfname"].ToString(),
                    StudentLName = ResultSet["studentlname"].ToString(),
                    StudentNumber = ResultSet["studentnumber"].ToString(),
                    EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]),
                };

                Students.Add(_Student);
            }
            Conn.Clone();
            return Students;
        }

        /// <summary>
        /// Gets a specific Student by its ID
        /// </summary>
        /// <param name="id">Integer. ID of the Student</param>
        /// <returns>Student Object</returns>
        [HttpGet]
        [Route("api/StudentData/SelectStudent/{id}")]
        public Student SelectStudent(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Students WHERE studentid = @id";

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            Student SelectedStudent = new Student(); ;

            while (ResultSet.Read())
            {
                Student _Student = new Student()
                {
                    StudentId = Convert.ToInt32(ResultSet["studentid"]),
                    StudentFName = ResultSet["studentfname"].ToString(),
                    StudentLName = ResultSet["studentlname"].ToString(),
                    StudentNumber = ResultSet["studentnumber"].ToString(),
                    EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]),
                };

                SelectedStudent = _Student;
            }
            Conn.Clone();
            return SelectedStudent;
        }

        /// <summary>
        /// Selects all column data from the Students database. Can be provided conditions after Select clause.
        /// </summary>
        /// <param name="conditions">Optional String. Additional clauses to add to Select statement</param>
        /// <example>
        /// SelectAllFromDB() -> All row data
        /// </example>
        /// <returns>List of Student objects.</returns>
        private List<Student> SelectAllFromDB(string conditions = null)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Students";
            if (conditions != null) query += " WHERE " + conditions;

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Student> Students = new List<Student>();

            while (ResultSet.Read())
            {
                Student _Student = new Student()
                {
                    StudentId = Convert.ToInt32(ResultSet["studentid"]),
                    StudentFName = ResultSet["studentfname"].ToString(),
                    StudentLName = ResultSet["studentlname"].ToString(),
                    StudentNumber = ResultSet["studentnumber"].ToString(),
                    EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]),
                };

                Students.Add(_Student);
            }
            Conn.Clone();
            return Students;
        }
    }
}
