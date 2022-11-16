using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using n01569183Cumulative1.Models;

namespace n01569183Cumulative1.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult List(string SearchParam = null)
        {
            StudentDataController StudentController = new StudentDataController();
            IEnumerable<Student> Students = StudentController.ListStudents(SearchParam);
            return View(Students);
        }

        public ActionResult Show(int id)
        {
            StudentDataController StudentController = new StudentDataController();
            Student SelectedStudent = StudentController.SelectStudent(id);
            return View(SelectedStudent);
        }
    }
}