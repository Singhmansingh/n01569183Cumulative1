﻿using Microsoft.Ajax.Utilities;
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
        [Route("api/TeacherData/ListTeachers/{SearchParam?}")]

        public IEnumerable<Teacher> ListTeachers(string SearchParam = null)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            string query = "SELECT * FROM Teachers";
            if (SearchParam != null) query = "SELECT * FROM Teachers WHERE lower(CONCAT(teacherfname, ' ', teacherlname)) LIKE @search";

            MySqlCommand cmd = Conn.CreateCommand();
            
            cmd.CommandText = query;
            if(SearchParam != null) cmd.Parameters.AddWithValue("@search", "%" + SearchParam + "%");
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

            string query = "SELECT * FROM Teachers WHERE teacherid = @id" ;

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
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
