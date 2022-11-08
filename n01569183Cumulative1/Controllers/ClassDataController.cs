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
    public class ClassDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Gets a list of all Classes
        /// </summary>
        /// <returns>List of type Class</returns>
        [HttpGet]
        public IEnumerable<Class> ListClasses()
        {
            return SelectAllFromDB();
        }

        /// <summary>
        /// Gets a specific Class by its ID
        /// </summary>
        /// <param name="id">Integer. ID of the Class</param>
        /// <returns>Class Object</returns>
        [HttpGet]
        [Route("api/ClassData/SelectClass/{id}")]
        public Class SelectClass(int id)
        {
            string conditions = $"classid = {id}";
            Class SelectedClass = SelectAllFromDB(conditions).First();
            return SelectedClass;
        }

        [HttpGet]
        [Route("api/ClassData/SelectClassByTeacherID/{id}")]

        public IEnumerable<Class> SelectClassByTeacherID(int id)
        {
            string conditions = $"teacherid = {id}";
            List<Class> Classes = SelectAllFromDB(conditions);
            return Classes;
        }

        /// <summary>
        /// Selects all column data from the Classes database. Can be provided conditions after Select clause.
        /// </summary>
        /// <param name="conditions">Optional String. Additional clauses to add to Select statement</param>
        /// <example>
        /// SelectAllFromDB() -> All row data
        /// </example>
        /// <returns>List of Class objects.</returns>
        private List<Class> SelectAllFromDB(string conditions = null)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Classes";
            if (conditions != null) query += " WHERE " + conditions;

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Class> Classes = new List<Class>();

            while (ResultSet.Read())
            {
                Class _class = new Class()
                {
                    ClassId = Convert.ToInt32(ResultSet["classid"]),
                    ClassCode = ResultSet["classcode"].ToString(),
                    TeacherId = Convert.ToInt32(ResultSet["teacherid"]),
                    StartDate = Convert.ToDateTime(ResultSet["startdate"]),
                    FinishDate = Convert.ToDateTime(ResultSet["finishdate"]),
                    ClassName = ResultSet["classname"].ToString()
                };

                Classes.Add(_class);
            }
            Conn.Clone();
            return Classes;
        }
    }
}
