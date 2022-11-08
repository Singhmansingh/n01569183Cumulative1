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
        public IEnumerable<Teacher> ListTeachers()
        {
            List<Teacher> Teachers = SelectAllFromDB();

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

            string conditions = "teacherid=" + id;
            Teacher SelectedTeacher = SelectAllFromDB(conditions).First();
            return SelectedTeacher;

        }

        /// <summary>
        /// Generates a list of Teachers that have the specified search term in their name
        /// </summary>
        /// <param name="name">string name to search</param>
        /// <example>
        /// GET: api/TeacherData/SearchTeacherByName/Joe -> List of Teachers whose full name (first name + last name)  contains the pattern "Joe"
        /// </example>
        /// <returns>List of Teacher objects</returns>
        [HttpGet]
        [Route("api/TeacherData/SearchTeacherByName/{name}")]
        public IEnumerable<Teacher> SearchTeacherByName(string name)
        {
            string conditions = $"CONCAT(teacherfname,teacherlname) LIKE '%{name}%'";
            
            List<Teacher> Teachers = SelectAllFromDB(conditions);

            return Teachers;
        }

        /// <summary>
        /// Generates a list of Teachers with a hiredate relative to the specified date
        /// </summary>
        /// <param name="year">string year of hire date (yyyy)</param>
        /// <param name="month">string month of hire date (MM)</param>
        /// <param name="day">string day of hire date (DD)</param>
        /// <param name="oppchoice">int operation choice.
        /// 1 -> ">" 
        /// 2 -> ">=" 
        /// -1 -> "<" 
        /// -2 -> "<=" 
        /// default -> "=" 
        /// </param>
        /// <example>
        /// GET: api/TeacherData/SearchTeacherByHireDate/2014/02/05/0 ->List of Teachers hired on 2014-02-05
        /// GET: api/TeacherData/SearchTeacherByHireDate/2014/02/05/-2 ->List of Teachers hired before or on 2014-02-05
        /// </example>
        /// <returns>List of Teacher objects</returns>
        [HttpGet]
        [Route("api/TeacherData/SearchTeacherByHireDate/{year}/{month}/{day}/{oppchoice}")]
        public IEnumerable<Teacher> SearchTeacherByHireDate(string year, string month, string day, int oppchoice)
        {
            string opperation = DefineOpperator(oppchoice);
            string conditions = $"hiredate {opperation} \"{year}-{month}-{day}\"";

            List<Teacher> Teachers = SelectAllFromDB(conditions);

            return Teachers;
        }

        /// <summary>
        /// Generates a list of Teachers that have a salary relative to the specified amount
        /// </summary>
        /// <param name="salary">decimal salary to search</param>
        /// <param name="oppchoice">int operation choice.
        /// 1 -> ">" 
        /// 2 -> ">=" 
        /// -1 -> "<" 
        /// -2 -> "<=" 
        /// default -> "=" 
        /// </param>
        /// <example>
        /// GET: api/TeacherData/SearchTeacherBySalary/32.00/0 -> List of Teachers whose salary is equal to 32.00
        /// GET: api/TeacherData/SearchTeacherBySalary/32.00/2 -> List of Teachers whose salary is greater than or equal to 32.00
        /// </example>
        /// <returns>List of Teacher objects</returns>
        [HttpGet]
        [Route("api/TeacherData/SearchTeacherBySalary/{salary}/{oppchoice}")]
        public IEnumerable<Teacher> SearchTeacherBySalary(decimal salary, int oppchoice)
        {
            string opperation = DefineOpperator(oppchoice);
            string conditions = $"salary {opperation} {salary}";

            List<Teacher> Teachers = SelectAllFromDB(conditions);

            return Teachers;
        }

        /// <summary>
        /// Selects all column data from the Teachers database. Can be provided conditions after Select clause.
        /// </summary>
        /// <param name="conditions">Optional String. Additional clauses to add to Select statement</param>
        /// <example>
        /// SelectAllFromDB() -> All row data
        /// </example>
        /// <returns>List of Teacher objects.</returns>
        private List<Teacher> SelectAllFromDB(string conditions = null)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Teachers";
            if (conditions != null) query += " WHERE " + conditions;

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
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
        /// Defines what conditional operator to use, based on integer choice.
        /// </summary>
        /// <param name="choice"> Integer. Which operator to use.</param>
        /// <returns></returns>
        private string DefineOpperator(int choice)
        {
            switch (choice)
            {
                case 1: return ">";
                case 2: return ">=";
                case -1: return "<";
                case -2: return "<=";
                default: return "=";
            }
        }

    }
}
