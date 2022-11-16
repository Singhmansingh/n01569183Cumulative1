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
    public class ClassDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Gets a list of all Classes
        /// </summary>
        /// <returns>List of type Class</returns>
        [HttpGet]
        [Route("api/ClassData/ListClasses/{SearchParam?}")]
        public IEnumerable<Class> ListClasses(string SearchParam = null)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Classes";
            if (SearchParam != null) query += " WHERE classcode LIKE @search OR classname LIKE @search";
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@search","%" + SearchParam + "%");
            cmd.Prepare();

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

        /// <summary>
        /// Gets a specific Class by its ID
        /// </summary>
        /// <param name="id">Integer. ID of the Class</param>
        /// <returns>Class Object</returns>
        [HttpGet]
        [Route("api/ClassData/SelectClass/{id}")]
        public Class SelectClass(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Classes WHERE classid = @id";

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            Class SelectedClass = new Class();

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

                SelectedClass = _class;
            }
            Conn.Clone();
            return SelectedClass;
        }
    }
}
